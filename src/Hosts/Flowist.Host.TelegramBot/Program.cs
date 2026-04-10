using Flowist.TelegramBot.Handlers;
using Flowist.TelegramBot.Options;
using Flowist.TelegramBot.Services;
using Flowist.Presentation.TelegramBot.Services;
using Flowlist.Core.Dependencies;
using Flowlist.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.creds.json", optional: true, reloadOnChange: true);
        })
        .ConfigureServices((context, services) =>
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            services.AddOptions<TelegramOptions>()
                .Bind(context.Configuration.GetSection("Telegram"))
                .Validate(options => !string.IsNullOrWhiteSpace(options.Token), "Token required")
                .ValidateOnStart();

            services.AddOptions<GatewayOptions>()
                .Bind(context.Configuration.GetSection(GatewayOptions.SectionName))
                .ValidateOnStart();

            services.AddHttpClient("ApiGateway", (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<GatewayOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            });

            services.AddScoped<IApiGatewayClient, ApiGatewayClient>();
            services.AddScoped<IMessageHandler, EchoMessageHandler>();
            services.AddHostedService<TelegramBotService>();
        })
        .Build();

    await builder.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}