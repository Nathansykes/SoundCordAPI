using Project.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.ConfigureLogging();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureApplication(builder.Configuration);

app.Run();
