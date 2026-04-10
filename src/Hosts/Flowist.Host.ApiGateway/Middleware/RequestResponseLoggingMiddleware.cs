using System.Text;
using Microsoft.IO;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _streamManager;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _streamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context.Request);
        
        var originalResponseBody = context.Response.Body;
        using var responseBody = _streamManager.GetStream();
        context.Response.Body = responseBody;

        await _next(context);

        await LogResponse(context.Response, responseBody);
        
        await responseBody.CopyToAsync(originalResponseBody);
    }

    private async Task LogRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;

        _logger.LogInformation(
            "Request: {Method} {Path} | Body: {Body}",
            request.Method, request.Path, body);
    }

    private async Task LogResponse(HttpResponse response, MemoryStream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation(
            "Response: {StatusCode} | Body: {Body}",
            response.StatusCode, body);
    }
}