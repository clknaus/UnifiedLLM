using Core.General.Models;
using Core.Supportive.Enums;
using Core.Supportive.Interfaces.DomainEvents;

namespace Application.Interfaces;
public interface IAsyncDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    int Order { get; }
    Task<Result<HandlerResult>> HandleAsync(TEvent domainEvent);
}
