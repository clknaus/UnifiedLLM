namespace Core.Domain.Events;
public class ChatCompletedEvent(Guid chatId) : DomainEventBase
{
    public Guid ChatId { get; } = chatId;
}
