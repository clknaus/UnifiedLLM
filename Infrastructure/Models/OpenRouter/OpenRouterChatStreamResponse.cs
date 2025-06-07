namespace Infrastructure.Models.OpenRouter;
public class OpenRouterChatStreamResponse
{
    public string Id { get; set; }
    public string Provider { get; set; }
    public string Model { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public IList<ChatStreamChoice> Choices { get; set; }
}
