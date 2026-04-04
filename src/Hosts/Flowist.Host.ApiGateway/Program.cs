using Flowlist.Core.Contracts;
using Flowlist.Core.Logger;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IConsoleLogger, DefaultConsoleLogger>();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapPost(
    "/ask",
    async ([FromBody] Message message, IConsoleLogger logger) =>
    {
        logger.LogInfo(
            string.Format(
                "Received message from user {0} in chat {1}: {2}",
                message.UserId,
                message.ChatId,
                message.Text
            )
        );
        return Results.Accepted(
            value: new
            {
                messageId = Guid.NewGuid(),
                status = "accepted",
                receivedAt = DateTime.UtcNow,
                text = message.Text,
            }
        );
    }
);

await app.RunAsync();
