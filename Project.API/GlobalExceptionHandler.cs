using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Project.Auth.Roles;
using Project.Domain.Exceptions;

namespace Project.API;

public class GlobalExceptionHandler(
    IWebHostEnvironment environment,
    ILogger<GlobalExceptionHandler> logger,
    IConfiguration configuration,
    ProblemDetailsFactory problemDetailsFactory) : IExceptionHandler
{
    private readonly IWebHostEnvironment _environment = environment;
    private readonly ILogger _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
            await HandleDomainException(httpContext, domainException, cancellationToken);
        else
            await HandleException(httpContext, exception, cancellationToken);

        return true;
    }

    private async Task HandleException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogException(exception, httpContext);
        if (ShowDeveloperError(httpContext))
            await WriteResponse(httpContext, StatusCodes.Status500InternalServerError, exception.Message, exception.StackTrace ?? "", cancellationToken);
        else
            await WriteResponse(httpContext, StatusCodes.Status500InternalServerError, "Server Error", "An internal server error occured, please contact an administrator", cancellationToken);
    }

    private async Task HandleDomainException(HttpContext httpContext, DomainException domainException, CancellationToken cancellationToken)
    {
        if (domainException.StatusCode >= 500 || _configuration.GetValue("DetailedExceptionLogging", false))
            LogException(domainException, httpContext);

        if (domainException is FriendlyException frEx && domainException.StatusCode < 300)
        {
            await WriteResponse(httpContext, frEx.StatusCode, frEx.Message, frEx.Description, cancellationToken);
            return;
        }

        if (ShowDeveloperError(httpContext))
            await WriteResponse(httpContext, domainException.StatusCode, domainException.Message, domainException.StackTrace ?? "", cancellationToken);
        else
            await WriteResponse(httpContext, domainException.StatusCode, "Request Error", domainException.Message, cancellationToken);
    }

    private bool ShowDeveloperError(HttpContext httpContext)
        => _environment.IsDevelopment()
        || _configuration.GetValue("DetailedExceptionLogging", false)
        || httpContext.User.IsInRole(UserRoles.Developer);

    private async Task WriteResponse(HttpContext httpContext, int statusCode, string title, string description, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext, statusCode: statusCode, title: title, detail: description);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
    private void LogException(Exception exception, HttpContext httpContext)
    {
        _logger.LogError(exception, "An exception of type {ExceptionTypeName} occurred. HttpRequest TraceIdentifier: {TraceIdentifier}", exception.GetType().Name, httpContext.TraceIdentifier);
    }
}
