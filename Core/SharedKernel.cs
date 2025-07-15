using Core.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Core;
// Shared Kernel
public abstract class Entity<TId> where TId : new()
{
    public TId Id { get; protected set; } = new();
    public string Hash { get; set; }

    protected Entity()
    {
        Id = (typeof(TId) == typeof(Guid)) ? (TId)(object)Guid.NewGuid() : default!; 
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public virtual void SetHash(string hash) => Hash = hash;
}

    //
    // TODO: sort IAggregateRoot and Domain Events in architecture layers.
    //
    public interface IAggregateRoot
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : new()
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}

//
// TODO: sort IRepository<T> in architecture layers.
//
public interface IRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(string id);
    Task SaveAsync(T entity);
}

// TODO: sort IUnitOfWork in architecture layers.

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}

// TODO: sort IDomainEventDispatcher in architecture layers.

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}

// TODO: sort DomainEventDispatcher Class along with Domain Events handler in architecture layers.

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        var domainHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var rawHandlers = _serviceProvider.GetServices(domainHandlerType);

        // guard
        if (rawHandlers is not IEnumerable enumerable)
            throw new ApplicationException();

        foreach (dynamic handler in enumerable)
            await handler.HandleAsync((dynamic)domainEvent);
    }
}

// Event Handler
// TODO: research logic on Event Handler, compare it with Service Injection style logic.
// TODO: try converting ChatCompletedEventHandler implementation to something more generic.
//* * * * * * * * * * * * * * * * *
//public abstract class DomainEvent
//{
//    public DateTime OccurredOn { get; } = DateTime.UtcNow;
//}

//public class ChatCycleCompletedEvent : DomainEvent
//{
//    public Guid ChatId { get; }
//    public ChatCycleCompletedEvent(Guid chatId) => ChatId = chatId;
//}

//public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
//{
//    Task HandleAsync(TEvent domainEvent);
//}
//* * * * * * * * * * * * * * * * *

public class ChatCompletedEventHandler : IDomainEventHandler<ChatCycleCompletedEvent>
{
    public Task HandleAsync(ChatCycleCompletedEvent domainEvent)
    {
        Console.WriteLine($"Chat with ID {domainEvent.ChatId} was completed at {domainEvent.OccurredOn}");
        return Task.CompletedTask;
    }
}


