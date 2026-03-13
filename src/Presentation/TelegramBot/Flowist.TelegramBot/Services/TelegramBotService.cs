using Flowist.TelegramBot.Options;
using Flowlist.Core.Interfaces;
using Flowlist.Core.Logger;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Flowist.Presentation.TelegramBot.Services;

public class TelegramBotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IConsoleLogger _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ReceiverOptions _receiverOptions;

    public TelegramBotService(
        IOptions<TelegramOptions> options,
        IConsoleLogger logger,
        IMessageHandler messageHandler
    )
    {
        _botClient = new TelegramBotClient(options.Value.Token);
        logger.LogTrace("TelegramBotService initialized with token: {0}", options.Value.Token);

        _logger = logger;
        _messageHandler = messageHandler;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message], // receive all update types
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await _botClient.GetMeAsync(stoppingToken);
        _logger.LogTrace($"Bot {me.Username} started.");

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: _receiverOptions,
            cancellationToken: stoppingToken
        );

        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        _logger.LogTrace($"=== {nameof(HandleUpdateAsync)} called ===");

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

        _logger.LogTrace($"Response sent to {message.From?.Username}");

        _logger.LogTrace($"=== {nameof(HandleUpdateAsync)} finished ===");
    }

    private Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Polling error: {0}", exception.Message);
        return Task.CompletedTask;
    }
}
