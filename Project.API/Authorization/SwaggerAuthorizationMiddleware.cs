using Microsoft.AspNetCore.Identity;
using Project.Auth;
using Project.Auth.Identity.Models;
using Project.Auth.Roles;
using System.Net;

namespace Project.API.Authorization;

public class SwaggerAuthorizationMiddleware(
    UserManager<ApplicationUser> userManager,
    AuthorizationExtensions authorizationExtensions,
    ILogger<SwaggerAuthorizationMiddleware> logger) : IMiddleware
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly AuthorizationExtensions _authorizationExtensions = authorizationExtensions;
    private readonly ILogger<SwaggerAuthorizationMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Request received {path}", context.Request.Path);
        if (!context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next.Invoke(context);
            return;
        }

        var user = await _authorizationExtensions.GetApplicationUserFromBasicAuthAsync(context);
        if (user is not null && await _userManager.IsInRoleAsync(user, UserRoles.Developer))
        {
            await next.Invoke(context);
            return;
        }

        context.Response.Headers.WWWAuthenticate = "Basic";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}