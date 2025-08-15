using Core.Supportive.Interfaces.DomainEvents;

namespace Core.General.Interfaces;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}