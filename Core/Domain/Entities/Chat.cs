using Core.General.Models;
using static Core.Domain.Entities.Tracker;

namespace Core.Domain.Entities;
public class Chat : AggregateRoot<Guid>
{
    internal Tracker Tracker { get; init; } = new Tracker();
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
        
        AddDomainEvent(new ChatCompletedEvent(Id));

        return Result<Chat>.Success(this);
    }
}
