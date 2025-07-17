namespace Core.Domain.Events;
public class ChatCycleCompletedEvent(Guid chatId) : DomainEventBase
{
    public Guid ChatId { get; } = chatId;
}
