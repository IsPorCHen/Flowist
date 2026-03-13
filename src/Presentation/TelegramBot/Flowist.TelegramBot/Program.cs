using Flowist.Presentation.TelegramBot.Services;
using Flowist.TelegramBot.Handlers;
using Flowist.TelegramBot.Options;
using Flowlist.Core.Dependencies;
using Flowlist.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(
        (context, config) =>
        {
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.creds.json", optional: true, reloadOnChange: true);
        }
    )
    .ConfigureServices(
        (context, services) =>
        {
            services.AddConsoleLogger();

            services
                .AddOptions<TelegramOptions>()
                .Bind(context.Configuration.GetSection("Telegram"))
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.Token),
                    "Telegram:Token must be configured"
                )
                .ValidateOnStart();

            // Handlers
            services.AddSingleton<IMessageHandler, EchoMessageHandler>();

            // Bot Service
            services.AddHostedService<TelegramBotService>();
        }
    )
    .Build();

await host.RunAsync();
