namespace Core.Supportive.Interfaces.DomainEvents;
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
    Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents);
}
