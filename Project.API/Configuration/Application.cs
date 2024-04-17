using Project.API.Authorization;
using Project.API.Hubs.Messages;

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

        app.UseCors("CorsPolicy");

        app.UseRouting();
        app.MapHub<MessageHub>("/messageshub");
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();


        return app;
    }
}
