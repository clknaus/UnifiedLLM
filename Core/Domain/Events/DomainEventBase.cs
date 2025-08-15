using Core.Supportive.Interfaces.DomainEvents;

namespace Core.Domain.Events;

public abstract class DomainEventBase : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int Version { get; } = 1;
    public IReadOnlyDictionary<string, object> Metadata { get; }
    protected DomainEventBase()
    {
        Metadata = new Dictionary<string, object>();
    }
    protected DomainEventBase(IReadOnlyDictionary<string, object>? metadata = null)
    {
        Metadata = metadata ?? new Dictionary<string, object>();
    }
}
