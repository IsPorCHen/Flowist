using Flowlist.Core.Interfaces;

namespace Flowist.Infrastructure.Config;

public class EnvBotConfig: IBotConfig
{
    public string TelegramToken => Environment.GetEnvironmentVariable("TELEGRAM BOT TOKEN")
        ?? throw new InvalidOperationException("Telegram bot token is not set in environment variables.");
}