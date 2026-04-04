using Flowlist.Core.Contracts;

namespace Flowlist.Core.Interfaces;

public interface IApiGatewayClient
{
    Task AskAsync(Message message, CancellationToken cancellationToken = default);
}