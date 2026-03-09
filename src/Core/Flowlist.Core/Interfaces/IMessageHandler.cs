using Flowlist.Core.Models;

namespace Flowlist.Core.Interfaces;

public interface IMessageHandler
{
    string HandleMessageAsync(string messageText, long ChatId);
}