namespace UnifiedLLM.Core.Models;
public class ChatResponse
{
    public string Id { get; set; }
    public IList<ChatChoice> Choices { get; set; }
}
