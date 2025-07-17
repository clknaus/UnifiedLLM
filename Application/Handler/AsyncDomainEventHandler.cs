using Application.Interfaces;
using Core.General.Models;
using Core.Supportive.Enums;
using Core.Supportive.Interfaces.DomainEvents;

namespace Application.Handler;
public abstract class AsyncDomainEventHandler<TEvent> : IAsyncDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public virtual int Order => 0;
    public abstract Task<Result<HandlerResult>> HandleAsync(TEvent domainEvent);
    protected Result<HandlerResult> Success => Result<HandlerResult>.Success(HandlerResult.Success);
}
