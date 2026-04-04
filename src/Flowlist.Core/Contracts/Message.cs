namespace Flowlist.Core.Contracts;

public record Message
{
    public long ChatId { get; set; }
    public long UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public MessageStatusCases Status { get; set; }
}

public enum MessageStatusCases
{
    Created,
    Updated,
}
