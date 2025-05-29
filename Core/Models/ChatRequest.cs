using Core.Models;

namespace UnifiedLLM.Core.Models;
public class ChatRequest
{
    public string Provider { get; set; }
    public string Model { get; set; }
    public IList<ChatMessage> Messages { get; set; }

    // Sampling parameters
    public double Temperature { get; set; } = 0.7;
    public double TopP { get; set; } = 1.0;
    public int N { get; set; } = 1;

    // Response control
    public int MaxTokens { get; set; } = 256;
    public bool Stream { get; set; } = false;
    public IList<string> Stop { get; set; }

    // Penalties
    public double PresencePenalty { get; set; } = 0.0;
    public double FrequencyPenalty { get; set; } = 0.0;

    // Token biasing
    public IDictionary<string, int> LogitBias { get; set; }

    // User identifier for tracking
    public string User { get; set; }

    // Function calling
    public IList<ToolDefinition> Tools { get; set; }
    public ToolChoice ToolChoice { get; set; }
}
