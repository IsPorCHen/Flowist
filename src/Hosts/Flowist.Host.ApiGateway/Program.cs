using Flowist.Host.ApiGateway.Filters;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithProperty("Timestamp", DateTime.UtcNow)
    // TODO: Write to file all logs, and only write to console logs with level >= Information
    .WriteTo.Console(
        // TODO: Config for information level and above in console
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile(
        "appsettings.creds.json",
        optional: true,
        reloadOnChange: true
    );

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();

    builder.Services.AddOpenApi();
    builder.Services.AddScoped<ApiKeyFilter>();

    var app = builder.Build();

    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    app.MapOpenApi();
    app.MapScalarApiReference();

    // TODO: Add health check endpoint that doesn't require API key for health checks
    app.MapAskEndpoint();

    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
