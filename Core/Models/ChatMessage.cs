namespace UnifiedLLM.Core.Models;
public class ChatMessage
{
    public string Role { get; set; } // "system", "user", or "assistant"
    public string Content { get; set; }
}
