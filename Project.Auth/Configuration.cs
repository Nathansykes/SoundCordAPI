using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Auth.Identity;
using Project.Auth.Identity.Models;
using Project.Auth.Roles;

namespace Project.Auth;
public static class Configuration
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Database")));

        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>()
                                      .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                                      .AddDefaultTokenProviders()
                                      .AddRoles<IdentityRole>();

        services.AddScoped<AuthorizationService>();
        services.AddScoped<RolesService>();
        return services;
    }
}
