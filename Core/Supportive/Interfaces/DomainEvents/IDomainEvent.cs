namespace Core.Supportive.Interfaces.DomainEvents;
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    int Version { get; }
    IReadOnlyDictionary<string, object> Metadata { get; }
}
