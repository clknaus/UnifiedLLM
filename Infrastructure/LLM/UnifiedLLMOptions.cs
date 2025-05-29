namespace API.Configuration;
public class UnifiedLLMOptions
{
    public const string SectionName = "UnifiedLLM";
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string ApiKey { get; set; } = "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";
    public double DefaultTemperature { get; set; } = 0.7;
    public int DefaultMaxTokens { get; set; } = 256;
}
