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

public interface IRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(string id);
    Task SaveAsync(T entity);
}

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var rawHandlers = _serviceProvider.GetServices(handlerType);

        if (rawHandlers is not IEnumerable enumerable)
            throw new ApplicationException();

        foreach (dynamic handler in enumerable)
            await handler.HandleAsync((dynamic)domainEvent);
    }
}

// Domain Event
public class ChatCycleCompletedEvent : IDomainEvent
{
    public Guid ChatId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ChatCycleCompletedEvent(Guid chatId)
    {
        ChatId = chatId;
    }
}

// Event Handler
public class ChatCompletedEventHandler : IDomainEventHandler<ChatCycleCompletedEvent>
{
    public Task HandleAsync(ChatCycleCompletedEvent domainEvent)
    {
        Console.WriteLine($"Chat with ID {domainEvent.ChatId} was completed at {domainEvent.OccurredOn}");
        return Task.CompletedTask;
    }
}


