using Flowlist.Core.Interfaces;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessageHandler : IMessageHandler
{
    private readonly IAppLogger _logger;
    public EchoMessageHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task<string> HandleMessageAsync(string messageText, string username, long ChatId)
    {
        _logger.LogInfo("Received message: {0} from chat: {1}", messageText, ChatId);
        return await Task.FromResult($"Echo: {messageText}");
    }
}