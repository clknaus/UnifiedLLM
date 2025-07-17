using Application.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Application.Handler;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        var domainHandlerType = typeof(IAsyncDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var rawHandlers = _serviceProvider.GetServices(domainHandlerType);

        // guard
        if (rawHandlers is not IEnumerable enumerable)
            throw new ApplicationException();

        foreach (dynamic handler in enumerable)
            await handler.HandleAsync((dynamic)domainEvent);
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            await DispatchAsync(domainEvent);
    }

    public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent => await DispatchAsync(domainEvent);
}
