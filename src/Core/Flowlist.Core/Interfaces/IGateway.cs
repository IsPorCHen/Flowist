namespace Flowlist.Core.Interfaces;

/// <summary>
/// contract for http communication with downstream services
/// </summary>

public interface IGateway
{
    /// <summary>
    /// sends http request returns response
    /// </summary>
    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default);
}