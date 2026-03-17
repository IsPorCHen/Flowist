namespace Flowlist.Core.Contracts;

public record Message
{
    public long ChatId { get; set; }
    public string Text { get; set; } = string.Empty;
}