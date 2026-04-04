using Flowlist.Core.Contracts;
using Flowlist.Core.Interfaces;
using Flowlist.Core.Logger;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessageHandler(IConsoleLogger logger, IApiGatewayClient apiGatewayClient) : IMessageHandler
{
    public async Task<string> HandleMessageAsync(string messageText, string username, long chatId)
    {
        logger.LogTrace("Received message: {0} from chat: {1}", messageText, chatId);

        var message = new Message()
        {
            ChatId = chatId,
            UserId = chatId,
            Text = messageText,
            Status = MessageStatusCases.Created
        };
        await apiGatewayClient.AskAsync(message);

        return await Task.FromResult($"Echo: {messageText}");
    }
}
