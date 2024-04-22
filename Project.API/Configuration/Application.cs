using Microsoft.EntityFrameworkCore;
using Project.API.Authorization;
using Project.API.Hubs.Messages;
using Project.Infrastructure.Model;

namespace Project.API.Configuration;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app, IConfiguration config)
    {
        app.UseExceptionHandler(_ => { });

        app.MapPost("ping", () => Results.Ok("pong"));

        app.MapPost("api/wakeup", async (IApplicationDbContext _context) =>
        {
            var g = await _context.Groups.FirstOrDefaultAsync();
            return Results.Ok("awake");
        });

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
