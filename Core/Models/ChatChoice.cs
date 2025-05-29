namespace UnifiedLLM.Core.Models;
public class ChatChoice
{
    public Delta Delta { get; set; }
    public int Index { get; set; }
    public string FinishReason { get; set; }
}
