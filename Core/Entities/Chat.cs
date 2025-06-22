using Core.Interfaces;

namespace Core.Entities;
public class Chat : BaseEntity
{
    public ChatRequest ChatRequest { get; init; }
    public ChatResponse ChatResponse { get; init; }

    public bool HasChatRequest => ChatRequest?.Id != null;
    public bool HasChatResponse => ChatResponse?.Id != null;
}
