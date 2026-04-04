using Flowlist.Core.Contracts;
using Flowlist.Core.Interfaces;
using Flowist.TelegramBot.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Flowist.TelegramBot.Services;

public class ApiGatewayClient(
    IHttpClientFactory httpClientFactory,
    IOptions<GatewayOptions> options,
    ILogger<ApiGatewayClient> logger) : IApiGatewayClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiGateway");
    private readonly string _gatewayUrl = options.Value.BaseUrl;

    public async Task AskAsync(Message message, CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Sending message to API Gateway: {Text}", message.Text);

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_gatewayUrl}/api/messages",
                message,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<object>(cancellationToken);
                logger.LogTrace("Message sent successfully: {Result}", result);
            }
            else
            {
                logger.LogWarning("Failed to send message. Status: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending message to API Gateway");
        }
    }
}