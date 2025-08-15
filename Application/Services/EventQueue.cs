using Application.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;

namespace Application.Services;

public class EventQueue : IEventQueue
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

