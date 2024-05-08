using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using Project.API.Hubs.Messages;
using Project.Application.Channels;
using Project.Application.Groups;
using Project.Application.Messages;
using Project.Application.Songs;
using Project.Auth;
using Project.Auth.Identity.Models;
using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Exceptions;
using Project.Domain.Files;
using Project.Domain.Files.FileUploading;
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
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .SetIsOriginAllowed(_ => true);
            });
        });
        services.ConfigureSignalR(config);


        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddScoped<AuthorizationExtensions>();
        services.AddRouting(options => options.LowercaseUrls = true);


        services.AddSingleton<IEmailClient, MailJetEmailClient>();
        services.AddSingleton<IEmailService, EmailService>();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        return services;
    }


    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IUserAccessValidator, UserAccessValidator>();
        services.AddClassesAsImplementedInterface(_applicationAssemblies, typeof(IModelMapper<,>));

        services.AddGroups();
        services.AddChannels();
        services.AddMessages();
        services.AddSongs();
        services.AddFiles();
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
        services.AddScoped<ISongService, SongService>();
        services.AddScoped<ISongRepository<Song>, SongRepository>();
        services.AddScoped<ISongRevisionService, SongRevisionService>();
        services.AddScoped<ISongRevisionRepository<SongRevision>, SongRevisionRepository>();
        return services;
    }
    public static IServiceCollection AddFiles(this IServiceCollection services)
    {
        services.AddSingleton<IFileUploadService, FileUploadService>();
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
        services.AddSingleton<HubExceptionFilter>();
        services.AddSignalR(options =>
        {
            options.AddFilter<HubExceptionFilter>();
        }).AddHubOptions<MessageHub>(options =>
        {
            options.EnableDetailedErrors = true;
        }).AddAzureSignalR(config["Azure:SignalR:ConnectionString"]);
        return services;
    }

}