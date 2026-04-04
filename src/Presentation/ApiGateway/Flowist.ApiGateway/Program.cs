using Flowlist.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/api/messages", async (
    [FromBody] Message message,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Received message from user {UserId} in chat {ChatId}: {Text}", 
        message.UserId, message.ChatId, message.Text);
    
    return Results.Accepted(value: new
    {
        messageId = Guid.NewGuid(),
        status = "accepted",
        receivedAt = DateTime.UtcNow,
        text = message.Text
    });
});

app.Run();