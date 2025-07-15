namespace Core.Domain.Events;
public class ChatCycleCompletedEvent : IDomainEvent
{
    public Guid ChatId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ChatCycleCompletedEvent(Guid chatId)
    {
        ChatId = chatId;
    }
}
