using Core.Domain.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;

namespace Core.Domain.Events;

public class DomainEventQueue : IDomainEventQueue
{
    private readonly List<IDomainEvent> _events = [];

    public void Enqueue(IDomainEvent domainEvent) => _events.Add(domainEvent);

    public IReadOnlyCollection<IDomainEvent> DequeueAll()
    {
        var copy = _events.ToList();
        _events.Clear();
        return copy;
    }
}

