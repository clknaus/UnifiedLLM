using Core.Supportive.Interfaces.DomainEvents;

namespace Core.Domain.Interfaces;

public interface IDomainEventQueue
{
    void Enqueue(IDomainEvent domainEvent);
    IReadOnlyCollection<IDomainEvent> DequeueAll();
}

