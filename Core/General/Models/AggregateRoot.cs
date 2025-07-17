using Core.General.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;

namespace Core.General.Models;
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : new()
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList().AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}