using Flowlist.Core.Interfaces;

// TelegramBot
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;

namespace Flowist.Presentation.TelegramBot.Services;

public class TelegramBotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IAppLogger _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ReceiverOptions _receiverOptions;

    public TelegramBotService(IBotConfig botConfig, IAppLogger logger, IMessageHandler messageHandler)
    {
        _botClient = new TelegramBotClient(botConfig.TelegramToken);
        _logger = logger;
        _messageHandler = messageHandler;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message } // receive all update types
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await _botClient.GetMeAsync(stoppingToken);
        _logger.LogInfo($"Bot {me.Username} started.");

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: _receiverOptions,
            cancellationToken: stoppingToken
        );

        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"=== HandleUpdateAsync called ===");

        if (update.Message is not { } message)
            return;

        _logger.LogInfo($"Received message from {message.From?.Username}: {message.Text}");

        var responseText = await _messageHandler.HandleMessageAsync(
            message.Text ?? string.Empty,
            message.From?.Username ?? "Unknown",
            message.Chat.Id
        );

        _logger.LogInfo($"Sending response to {message.From?.Username}: {responseText}");

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: responseText,
            cancellationToken: cancellationToken
        );

        _logger.LogInfo($"Response sent to {message.From?.Username}");
    }
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError("Polling error: {ExceptionMessage}", exception.Message);
        return Task.CompletedTask;
    }
}
