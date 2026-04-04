namespace Flowist.TelegramBot.Options;

public class GatewayOptions
{
    public const string SectionName = "Gateway";
    public string BaseUrl { get; set; } = "http://api-gateway:5000";
    public int TimeoutSeconds { get; set; } = 30;
}