using Flowlist.Core.Interfaces;
using Flowlist.Core.Models;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessagehandler : IMessageHandler
{
    private readonly IAppLogger _logger;
    public EchoMessagehandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public string HandleMessageAsync(string messageText, long ChatId)
    {
        _logger.LogInfo("Received message: {Message} from chat: {ChatId}", messageText, ChatId);
        return $"Echo: {messageText}";
    }
}