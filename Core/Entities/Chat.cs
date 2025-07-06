using Core.Models;

namespace Core.Entities;
public class Chat : AggregateRoot<Guid>
{
    internal Tracker Tracker { get; set; }
    public ChatRequest ChatRequest { get; init; }
    public ChatResponse ChatResponse { get; init; }

    public bool HasChatRequest => ChatRequest?.Id != null;
    public bool HasChatResponse => ChatResponse?.Id != null;

    public Result<Chat> ValidateThenRaiseEvent()
    {
        if (!HasChatRequest)
            return Result<Chat>.Failure("ChatRequest is missing.");

        if (!HasChatResponse)
            return Result<Chat>.Failure("ChatResponse is missing.");
        
        AddDomainEvent(new ChatCompletedEvent(base.Id));

        return Result<Chat>.Success(this);
    }
}
