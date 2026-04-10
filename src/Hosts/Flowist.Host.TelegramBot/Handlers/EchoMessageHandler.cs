using Flowlist.Core.Contracts;
using Flowlist.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Flowist.TelegramBot.Handlers;

public class EchoMessageHandler(
    ILogger<EchoMessageHandler> logger,
    IApiGatewayClient apiGatewayClient) : IMessageHandler
{
    public async Task<string> HandleMessageAsync(string messageText, string username, long chatId)
    {
        logger.LogTrace("Received message: {Text} from chat: {ChatId}", messageText, chatId);

        var message = new Message()
        {
            ChatId = chatId,
            UserId = chatId,
            Text = messageText,
            Status = MessageStatusCases.Created
        };
        await apiGatewayClient.AskAsync(message);

        return $"Echo: {messageText}";
    }
}