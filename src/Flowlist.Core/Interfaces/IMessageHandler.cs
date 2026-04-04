namespace Flowlist.Core.Interfaces;

public interface IMessageHandler
{
    Task<string> HandleMessageAsync(string messageText, string username, long chatId);
}
