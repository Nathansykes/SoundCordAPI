﻿using Project.Auth;
using Project.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Reflection;
using Project.Infrastructure.Model;
using Project.Domain;
using Microsoft.AspNetCore.Identity;
using Project.Auth.Identity.Models;
using Project.Domain.Groups;
using Project.Application.Groups;
using Project.Infrastructure.Model.Entities;
using Project.Infrastructure.Repositories;

namespace Project.API.Configuration;


public static class ServicesExtensions
{
    private static readonly Assembly[] _applicationAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(x => x.FullName is not null && x.FullName.StartsWith("Project.")).ToArray();

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication(config);
        services.AddSwagger();
        services.AddIdentity(config);
        services.AddInfrastructure(config);
        return services;
    }

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Logging.AddAzureWebAppDiagnostics();
        if (builder.Environment.IsProduction())
        {
            builder.Logging.AddApplicationInsights(config =>
            {
                config.ConnectionString = builder.Configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");
            }, options => { });
        }

        builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("AzureLogging"));
        builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("DC", LogLevel.Trace);
        return builder;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions.Remove("traceId");
                context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddMemoryCache();
        services.AddCors();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddScoped<AuthorizationExtensions>();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>(services =>
        {
            var httpContext = services.GetRequiredService<IHttpContextAccessor>();
            var user = httpContext.HttpContext?.User;
            if (user is null || user.Identity?.Name is null || (!user.Identity.IsAuthenticated))
            {
                throw new InvalidOperationException();
            }

            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
            var appUser = new ApplicationUserModel
            {
                Id = userMgr.GetUserId(user)!,
                UserName = userMgr.GetUserName(user)!
            };
            return new CurrentUserAccessor(appUser);
        });

        services.AddClassesAsImplementedInterface(_applicationAssemblies, typeof(IModelMapper<,>));
        
        services.AddGroups();

        return services;
    }

    public static IServiceCollection AddGroups(this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IGroupRepository<Group>, GroupRepository>();
        return services;
    }


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
                   .UseSqlServer(config.GetConnectionString("Database")));

        services.AddDbContext<IUserApplicationDbContext, UserApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
                   .UseSqlServer(config.GetConnectionString("Database")));

        return services;
    }

    
}