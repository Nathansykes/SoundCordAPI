using Project.Auth;
using Project.Domain.Repositories;
using Project.Generic;
using Project.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Reflection;

namespace Project.Application.Configuration;


public static class ServicesExtensions
{
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

    public static void AddApplication(this IServiceCollection services, IConfiguration config)
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
    }


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Database")));

        services.AddClassesAsImplementedInterface(Assembly.GetAssembly(typeof(IApplicationDbContext))!, typeof(IRepository<,>));

        return services;
    }

    public static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
    {
        var typeInfoList = assembly.DefinedTypes.Where(x =>
        {
            return x.IsClass && !x.IsAbstract && x != compareType
                    && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == compareType);
        }).ToList();

        return typeInfoList;
    }

    public static void AddClassesAsImplementedInterface(
            this IServiceCollection services,
            Assembly assembly,
            Type compareType,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        assembly.GetTypesAssignableTo(compareType).ForEach((type) =>
        {
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementedInterface, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementedInterface, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementedInterface, type);
                        break;
                }
            }
        });
    }
}
