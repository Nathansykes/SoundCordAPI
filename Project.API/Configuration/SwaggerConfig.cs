using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Project.API.Authorization;
using Project.Auth;
using Project.Auth.Identity.Models;
using Project.Auth.Roles;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Project.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddScoped<SwaggerAuthorizationFilter>();
        services.AddScoped<SwaggerAuthorizationMiddleware>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.AddSecurityDefinition(IdentityConstants.BearerScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Type = SecuritySchemeType.Http,
                BearerFormat = "AspNet",
                Scheme = "Bearer",
            });
            options.DocumentFilter<SwaggerAuthorizationFilter>();
            options.AddSignalRSwaggerGen();
        });
        return services;
    }
}

class SwaggerAuthorizationFilter(IServiceProvider serviceProvider) : IDocumentFilter
{
    private IHttpContextAccessor _httpContextAccessor = default!;
    private UserManager<ApplicationUser> _userManager = default!;
    private AuthorizationExtensions _authorizationExtensions = default!;

    private static readonly string[] HiddenRoles = [UserRoles.GlobalAdmin, UserRoles.Developer, UserRoles.Admin];
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private void InitializeServices()
    {
        _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        _userManager = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        _authorizationExtensions = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<AuthorizationExtensions>();
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        InitializeServices();
        List<string> userRoles;
        try
        {
            userRoles = Task.Run(GetUserRoles).GetAwaiter().GetResult().ToList();
        }
        catch
        {
            userRoles = [];
        }

        var apiDescriptions = context.ApiDescriptions.Where(path => path.ActionDescriptor.EndpointMetadata.Any(attr => attr is AuthorizeAttribute)).ToList();
        foreach (var apiDescription in apiDescriptions)
        {
            var operations = swaggerDoc.Paths["/" + apiDescription.RelativePath]?.Operations;
            if (operations is null)
                continue;

            var (operationKey, operation) = operations.FirstOrDefault(op => op.Key.ToString().Equals(apiDescription.HttpMethod, StringComparison.OrdinalIgnoreCase));
            if (operation is null)
                continue;

            var authAttributes = apiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().ToList();
            var anonymousAttributes = apiDescription.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().ToList();

            if (authAttributes.Count != 0 && anonymousAttributes.Count == 0)
            {
                operation.Security.Add(BearerRequirement);
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

                var roles = authAttributes
                        .Where(attr => !string.IsNullOrWhiteSpace(attr.Roles))
                        .SelectMany(attr => attr.Roles!.Split(","))
                        .ToList();

                if (roles.Count != 0)
                {
                    operation.Responses.Add("403", new OpenApiResponse { Description = $"Forbidden, Required Roles: {string.Join(", ", roles)}" });
                    foreach (var role in roles)
                    {
                        if (!userRoles.Contains(role))
                        {
                            operations.Remove(operationKey);
                        }
                    }
                }
            }
        }

        foreach (var (pathUri, pathItem) in swaggerDoc.Paths)
        {
            if (pathItem.Operations.Count == 0)
            {
                swaggerDoc.Paths.Remove(pathUri);
            }
        }
    }

    private async Task<IList<string>> GetUserRoles()
    {
        if (_httpContextAccessor.HttpContext is null)
            return [];
        var user = await _authorizationExtensions.GetApplicationUserFromBasicAuthAsync(_httpContextAccessor.HttpContext);
        if (user is null)
            return [];
        return await _userManager.GetRolesAsync(user);
    }

    private OpenApiSecurityRequirement BearerRequirement => new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = IdentityConstants.BearerScheme
                }
            },
            []
        }
    };
}