using Flowlist.Core.Interfaces;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessageHandler : IMessageHandler
{
    private readonly IAppLogger _logger;
    public EchoMessageHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public Task<string> HandleMessageAsync(string messageText, string username, long ChatId)
    {
        _logger.LogInfo("Received message: {Message} from chat: {ChatId}", messageText, ChatId);
        return Task.FromResult($"Echo: {messageText}");
    }
}