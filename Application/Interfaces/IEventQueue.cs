using Core.Supportive.Interfaces.DomainEvents;

namespace Application.Interfaces;

public interface IEventQueue
{
    void Enqueue(IDomainEvent domainEvent);
    IReadOnlyCollection<IDomainEvent> DequeueAll();
}

