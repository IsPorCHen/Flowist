using Flowist.TelegramBot.Options;
using Flowlist.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Flowist.Presentation.TelegramBot.Services;

public class TelegramBotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ReceiverOptions _receiverOptions;

    public TelegramBotService(
        IOptions<TelegramOptions> options,
        ILogger<TelegramBotService> logger,
        IMessageHandler messageHandler
    )
    {
        _botClient = new TelegramBotClient(options.Value.Token);
        _logger = logger;
        _messageHandler = messageHandler;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await _botClient.GetMeAsync(stoppingToken);
        _logger.LogTrace("Bot {Username} started.", me.Username);

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
        _logger.LogTrace("=== {MethodName} called ===", nameof(HandleUpdateAsync));

        if (update.Message is not { } message)
            return;

        _logger.LogInformation("Received message from {Username}: {Text}", message.From?.Username, message.Text);

        var responseText = await _messageHandler.HandleMessageAsync(
            message.Text ?? string.Empty,
            message.From?.Username ?? "Unknown",
            message.Chat.Id
        );

        _logger.LogInformation("Sending response to {Username}: {Response}", message.From?.Username, responseText);

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: responseText,
            cancellationToken: cancellationToken
        );

        _logger.LogTrace("Response sent to {Username}", message.From?.Username);
        _logger.LogTrace("=== {MethodName} finished ===", nameof(HandleUpdateAsync));
    }

    private Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Polling error: {Message}", exception.Message);
        return Task.CompletedTask;
    }
}