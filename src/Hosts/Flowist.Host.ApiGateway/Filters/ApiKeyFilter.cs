public class ApiKeyFilter(IConfiguration configuration) : IEndpointFilter
{
    private readonly string? _apiKey = configuration["ApiKeyOptions:ApiKey"];

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (string.IsNullOrEmpty(_apiKey))
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            return Results.Unauthorized();

        if (!_apiKey.Equals(extractedApiKey))
            return Results.Unauthorized();

        return await next(context);
    }
}