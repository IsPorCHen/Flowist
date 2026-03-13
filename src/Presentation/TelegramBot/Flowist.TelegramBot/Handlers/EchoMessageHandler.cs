using Flowlist.Core.Interfaces;
using Flowlist.Core.Logger;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessageHandler(IConsoleLogger logger) : IMessageHandler
{
    public async Task<string> HandleMessageAsync(string messageText, string username, long chatId)
    {
        logger.LogTrace("Received message: {0} from chat: {1}", messageText, chatId);

        return await Task.FromResult($"Echo: {messageText}");
    }
}
