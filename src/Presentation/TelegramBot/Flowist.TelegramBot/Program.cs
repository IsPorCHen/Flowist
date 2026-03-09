using Flowist.Infrastructure.Config;
using Flowist.Infrastructure.Logging;
using Flowist.TelegramBot.Handlers;
using Flowlist.Core.Interfaces;
using Flowist.Presentation.TelegramBot.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Flowist.TelegramBot;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Configure Telegram Bot Client
                services.AddSingleton<IBotConfig, EnvBotConfig>();

                // Logging
                services.AddSingleton<IAppLogger, ConsoleLogger>();

                // Handlers
                services.AddSingleton<IMessageHandler, EchoMessageHandler>();

                // Bot Service
                services.AddHostedService<TelegramBotService>();
            });
}