using Flowlist.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithProperty("Timestamp", DateTime.UtcNow)
    // todo: add file sink with rolling interval
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" // write to file again
    )
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile("appsettings.creds.json", optional: true, reloadOnChange: true);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();

    builder.Services.AddOpenApi();
    builder.Services.AddScoped<ApiKeyFilter>();

    var app = builder.Build();

    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapAskEndpoint();

    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}