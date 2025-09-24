using Core.Domain.Events;
using Core.General.Models;

namespace Core.Domain.Entities;
public class Chat : AggregateRoot<Guid, Chat>
{
    public ChatRequest? ChatRequest { get; init; }
    public ChatResponse? ChatResponse { get; set; }

    public bool HasChatRequest => ChatRequest?.Id != null;
    public bool HasChatResponse => ChatResponse?.Id != null;

    public override Result<Chat> ValidateThenRaiseEvent()
    {
        if (!HasChatRequest)
            return Result<Chat>.Failure("ChatRequest (input) is missing.");

        if (!HasChatResponse)
            return Result<Chat>.Failure("ChatResponse is missing.");

        AddDomainEvent(new ChatCompletedEvent(Id));

        return Result<Chat>.Success(this);
    }
}
