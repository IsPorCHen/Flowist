using Flowist.Core.Models;

namespace Flowist.Core.Interfaces;

public interface IMessageHandler
{
    string HandleMessageAsync(string messageText, long ChatId);
}