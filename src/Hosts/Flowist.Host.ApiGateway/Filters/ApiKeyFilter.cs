namespace Flowist.Host.ApiGateway.Filters;

public class ApiKeyFilter(IConfiguration configuration) : IEndpointFilter
{
    private readonly string? _apiKey = configuration["ApiKeyOptions:ApiKey"];

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        if (string.IsNullOrEmpty(_apiKey))
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        // TODO: Support /health endpoint without API key for health checks
        // TODO: Check ignore case for X-API-KEY header

        if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            return Results.Unauthorized();

        if (!_apiKey.Equals(extractedApiKey))
            return Results.Unauthorized();

        return await next(context);
    }
}
