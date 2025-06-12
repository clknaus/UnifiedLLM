namespace Infrastructure.Models.OpenRouter;
public class OpenRouterConfiguration
{
    public const string SectionName = "UnifiedLLM";
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/";
    public string ApiKey { get; set; } = string.Empty;
    public double DefaultTemperature { get; set; } = 0.1;
    public int DefaultMaxTokens { get; set; } = 256;
}
