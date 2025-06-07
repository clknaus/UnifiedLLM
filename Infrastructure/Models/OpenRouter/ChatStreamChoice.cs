namespace Infrastructure.Models.OpenRouter;
public class ChatStreamChoice
{
    public int Index { get; set; }
    public Delta Delta { get; set; }
    public string? FinishReason { get; set; }
    public string? NativeFinishReason { get; set; }
    public object? Logprobs { get; set; }
}
