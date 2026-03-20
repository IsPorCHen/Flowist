using System.Text;
using System.Text.Json;
using Flowlist.Core.Interfaces;

namespace Flowlist.Core.Extensions;

public static class GatewayExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true // Name == name
    };

    // GET request
    public static async Task<HttpResponseMessage> GetAsync(
        this IGateway gateway,
        string path,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await gateway.SendAsync(request, cancellationToken);
    }

    // GET request deserialize
    public static async Task<T?> GetAsync<T> (
        this IGateway gateway,
        string path,
        CancellationToken cancellationToken = default) where T : class
    {
        var response = await gateway.GetAsync(path, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;
        
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    // POST request
    public static async Task<HttpResponseMessage> PostAsync(
        this IGateway gateway,
        string path,
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = content
        };
        return await gateway.SendAsync(request, cancellationToken);
    }

    // POST with JSON
    public static async Task<HttpResponseMessage> PostJsonAsync<T>(
        this IGateway gateway,
        string path,
        T data,
        CancellationToken cancellationToken = default) where T : class
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await gateway.PostAsync(path, content, cancellationToken);
    } 

    // POST with JSON & answer
    public static async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(
        this IGateway gateway,
        string path,
        TRequest data,
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class
    {
        var response = await gateway.PostJsonAsync(path, data, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;
        
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
    }
}