﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using Project.API.Hubs.Messages;
using Project.Application.Channels;
using Project.Application.Groups;
using Project.Application.Messages;
using Project.Auth;
using Project.Auth.Identity.Models;
using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Files;
using Project.Domain.Groups;
using Project.Domain.Messages;
using Project.Domain.Songs;
using Project.Generic;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;
using Project.Infrastructure.Repositories;
using System.Reflection;

namespace Project.API.Configuration;

public static class ServicesExtensions
{
    private static readonly Assembly[] _applicationAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(x => x.FullName is not null && x.FullName.StartsWith("Project.")).ToArray();

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplication(config);
        services.AddDomain();
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
        services.AddRouting(options => options.LowercaseUrls = true);

        services.ConfigureSignalR(config);

        services.AddSingleton<FileUploadService>();

        return services;
    }


    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>(services =>
        {
            var httpContext = services.GetRequiredService<IHttpContextAccessor>();
            var user = httpContext.HttpContext?.User;
            if (user is null || user.Identity?.Name is null || (!user.Identity.IsAuthenticated))
            {
                throw new InvalidOperationException("Request is not authenticated, cannot access current user");
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
        services.AddChannels();
        services.AddMessages();
        return services;
    }

    public static IServiceCollection AddGroups(this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IGroupRepository<Group>, GroupRepository>();
        return services;
    }
    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IChannelRepository<Channel>, ChannelRepository>();
        return services;
    }
    public static IServiceCollection AddMessages(this IServiceCollection services)
    {
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IMessageRepository<Message>, MessageRepository>();
        return services;
    }
    public static IServiceCollection AddSongs(this IServiceCollection services)
    {
        services.AddScoped<ISongRepository<Song>, SongRepository>();
        services.AddScoped<ISongRevisionRepository<SongRevision>, SongRevisionRepository>();
        return services;
    }
    public static IServiceCollection AddFiles(this IServiceCollection services)
    {
        services.AddScoped<IFileMetadataRepository<FileMetadatum>, FileMetadataRepository>();
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


    public static IServiceCollection ConfigureSignalR(this IServiceCollection services, IConfiguration config)
    {
        services.AddSignalR().AddHubOptions<MessageHub>(options =>
        {

        });
        return services;
    }

}