using Project.API.Authorization;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Project.API.Configuration;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app, IConfiguration config)
    {
        app.UseExceptionHandler(_ => { });

        app.MapPost("ping", () => Results.Ok("pong"));

        app.UseMiddleware<SwaggerAuthorizationMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
