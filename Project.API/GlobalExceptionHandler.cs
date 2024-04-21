using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Project.Auth.Roles;
using Project.Domain.Exceptions;
using Project.Generic;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Project.API;

public class GlobalExceptionHandler(
    IWebHostEnvironment environment,
    ILogger<GlobalExceptionHandler> logger,
    IConfiguration configuration,
    ProblemDetailsFactory problemDetailsFactory,
    EmailService emailService,
    IWebHostEnvironment webHostEnvironment) : IExceptionHandler
{
    private readonly IWebHostEnvironment _environment = environment;
    private readonly ILogger _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    private readonly EmailService _emailService = emailService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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
        await LogException(exception, httpContext);
        if (true || ShowDeveloperError(httpContext))
            await WriteResponse(httpContext, StatusCodes.Status500InternalServerError, exception.Message, exception.StackTrace ?? "", cancellationToken);
        else
            await WriteResponse(httpContext, StatusCodes.Status500InternalServerError, "Server Error", "An internal server error occured, please contact an administrator", cancellationToken);
    }

    private async Task HandleDomainException(HttpContext httpContext, DomainException domainException, CancellationToken cancellationToken)
    {
        if (domainException.StatusCode >= 500 || _configuration.GetValue("DetailedExceptionLogging", false))
            await LogException(domainException, httpContext);

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
    private async Task LogException(Exception exception, HttpContext httpContext)
    {
        _logger.LogError(exception, "An exception of type {ExceptionTypeName} occurred. HttpRequest TraceIdentifier: {TraceIdentifier}", exception.GetType().Name, httpContext.TraceIdentifier);
        var exceptionString = BuildExceptionString(httpContext, exception).ReplaceLineEndings("<br />");
        var exceptionSubject = $"{_webHostEnvironment.EnvironmentName}; Type: {exception.GetType().Name}; Time: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss:ff}";
        await _emailService.SendEmail("nathansykes10@hotmail.com", exceptionSubject, exceptionString);
    }

    public string BuildExceptionString(HttpContext httpContext, Exception exception)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=========== Request Information ===========");
        sb.AppendLine($"Request TraceIdentifier: {httpContext.TraceIdentifier}");
        sb.AppendLine($"Request Method: {httpContext.Request.Method}");
        sb.AppendLine($"Request Url: {httpContext.Request.GetDisplayUrl()}");
        sb.AppendLine($"Request IP: {httpContext.Connection.RemoteIpAddress}");
        sb.AppendLine($"Http Status Code: {httpContext.Response.StatusCode}");
        sb.AppendLine("=========== Host Information ===========");
        sb.AppendLine($"Environment: {_webHostEnvironment.EnvironmentName}");
        sb.AppendLine("Calling Assembly: " + GetExceptionLocationAssemblyName(exception));
        sb.AppendLine($"Application Name: {_webHostEnvironment.ApplicationName}");
        sb.AppendLine($"Computer Name: {Environment.MachineName}");
        sb.AppendLine($"Host Addresses:");
        foreach (var address in Dns.GetHostAddresses(Dns.GetHostName()))
            sb.AppendLine($"\t - {address}");
        sb.ToString();

        sb.AppendLine("=========== Exception Information ===========");
        sb.AppendLine(BuildStackTrace(exception));

        return sb.ToString();
    }

    public string BuildStackTrace(Exception exception)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<hr /> =========== Stack Trace =========== <hr />");
        int innerExceptionCount = 0;
        BuildStackTraceRecursive(exception, ref innerExceptionCount, sb);
        if (innerExceptionCount >= 100)
            sb.AppendLine("More than 100 inner exceptions thrown, only showing first 100");
        return sb.ToString();
    }

    private static void BuildStackTraceRecursive(Exception exception, ref int innerExceptionCount, StringBuilder sb)
    {
        if (innerExceptionCount >= 100)
            return;
        innerExceptionCount++;

        sb.AppendLine("Exception Type: " + exception.GetType().Name);
        sb.AppendLine("Message: " + exception.Message);
        sb.AppendLine("Exception Data:");
        foreach (DictionaryEntry de in exception.Data)
            sb.AppendLine($"\t - {de.Key}: {de.Value}");

        var stackTraceHtml = exception.StackTrace?.Replace("<", "&lt;").Replace(">", "&gt;").Replace(" at ", "\r\n at ");
        sb.AppendLine(stackTraceHtml);
        sb.AppendLine("==============================");

        if (exception.InnerException != null)
            BuildStackTraceRecursive(exception.InnerException, ref innerExceptionCount, sb);

        if (exception is AggregateException aggregateException)
        {
            foreach (var inner in aggregateException.InnerExceptions)
                BuildStackTraceRecursive(inner, ref innerExceptionCount, sb);
        }

    }

    public string? GetExceptionLocationAssemblyName(Exception exception)
    {
        var localAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName?.Contains("project.", StringComparison.OrdinalIgnoreCase) ?? false)
            .Select(x => x.FullName ?? "")
            .ToList();

        string? errorLocation = null;
        if (exception.StackTrace is not null)
        {
            foreach (var line in exception.StackTrace.Split("\n"))
            {
                errorLocation = localAssemblies.FirstOrDefault(y => line?.Contains(y, StringComparison.OrdinalIgnoreCase) ?? false);
                if (errorLocation is not null)
                    break;
            }
        }

        return errorLocation;
    }
}
