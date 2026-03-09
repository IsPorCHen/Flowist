using Flowlist.Core.Interfaces;
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
        _logger = logger;
        _logger.LogInfo("TelegramBotService constructor starting...");
        
        try
        {
            _botClient = new TelegramBotClient(botConfig.TelegramToken);
            _logger.LogInfo("TelegramBotClient created successfully");
            
            _messageHandler = messageHandler;
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            };
            
            _logger.LogInfo("TelegramBotService constructor completed");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in constructor: {ex.Message}");
            throw;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInfo("ExecuteAsync started");
        
        try
        {
            var me = await _botClient.GetMeAsync(stoppingToken);
            _logger.LogInfo($"Bot {me.Username} started successfully!");
            
            _logger.LogInfo("Starting to receive messages...");
            
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: stoppingToken
            );
            
            _logger.LogInfo("StartReceiving called, bot is now listening");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in ExecuteAsync: {ex.Message}");
            throw;
        }

        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInfo("========== NEW UPDATE RECEIVED ==========");
        _logger.LogInfo($"Update type: {update.Type}");
        
        try
        {
            if (update.Message is not { } message)
            {
                _logger.LogInfo("Update is not a message, ignoring");
                return;
            }

            _logger.LogInfo($"Message details:");
            _logger.LogInfo($"  - Text: '{message.Text}'");
            _logger.LogInfo($"  - From: @{message.From?.Username}");
            _logger.LogInfo($"  - Chat ID: {message.Chat.Id}");
            _logger.LogInfo($"  - Chat type: {message.Chat.Type}");

            _logger.LogInfo("Calling message handler...");
            var responseText = await _messageHandler.HandleMessageAsync(
                message.Text ?? string.Empty,
                message.From?.Username ?? "Unknown",
                message.Chat.Id
            );
            _logger.LogInfo($"Handler returned: '{responseText}'");

            _logger.LogInfo("Sending response to Telegram...");
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: responseText,
                cancellationToken: cancellationToken
            );
            
            _logger.LogInfo("✅ Response sent successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ ERROR in HandleUpdateAsync: {ex.Message}");
            _logger.LogError($"Stack trace: {ex.StackTrace}");
        }
        
        _logger.LogInfo("========== UPDATE PROCESSING COMPLETED ==========");
    }
    
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError($"POLLING ERROR: {exception.Message}");
        _logger.LogError($"Stack trace: {exception.StackTrace}");
        return Task.CompletedTask;
    }
}