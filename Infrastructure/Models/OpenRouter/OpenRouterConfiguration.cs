namespace Infrastructure.Models.OpenRouter;
public class OpenRouterConfiguration
{
    public const string SectionName = "UnifiedLLM";
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/";
    public string ApiKey { get; set; } = "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";
    public double DefaultTemperature { get; set; } = 0.1;
    public int DefaultMaxTokens { get; set; } = 256;
}
