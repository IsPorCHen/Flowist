using Flowlist.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

public static class AskEndpoint
{
    public static void MapAskEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/ask", HandleAskAsync)
           .AddEndpointFilter<ApiKeyFilter>();
    }

    private static async Task<IResult> HandleAskAsync(
        [FromBody] Message message,
        ILogger<Program> logger)
    {
        logger.LogInformation(
            "Received message from user {UserId} in chat {ChatId}: {Text}",
            message.UserId, message.ChatId, message.Text);

        var messageId = Guid.NewGuid();
        var response = new
        {
            messageId = messageId,
            status = "accepted",
            receivedAt = DateTime.UtcNow,
            text = message.Text
        };

        return Results.Accepted($"/messages/{messageId}", response);
    }
}