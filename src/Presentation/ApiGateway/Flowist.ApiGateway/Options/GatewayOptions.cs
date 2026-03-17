namespace Flowist.ApiGateway.Options;

public class GatewayOptions
{
    public const string SectionName = "Gateway";
    public List<RouteConfig> Routes { get; set; } = new();

    public class RouteConfig
    {
        public string Path { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty();
        public string[] Methods { get; set; } = {"GET", "POST"};
    }
}