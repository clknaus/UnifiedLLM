using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.General.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IDomainEventQueue _eventQueue;

    public EfUnitOfWork(AppDbContext context, IDomainEventDispatcher dispatcher, IDomainEventQueue eventQueue)
    {
        _context = context;
        _dispatcher = dispatcher;
        _eventQueue = eventQueue;
    }

    public async Task<int> CommitAsync()
    {
        var aggregates = _context.ChangeTracker.Entries()
            .Where(e => e.Entity is IAggregateRoot)
            .Select(e => (IAggregateRoot)e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .ToList();

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.GetDomainEvents())
            {
                var outboxEvent = DomainEventSerializer.ToOutboxEvent(domainEvent);
                await _context.Set<OutboxEvent>().AddAsync(outboxEvent);
            }

            aggregate.ClearDomainEvents();
        }

        foreach (var domainEvent in _eventQueue.DequeueAll())
        {
            var outboxEvent = DomainEventSerializer.ToOutboxEvent(domainEvent);
            await _context.Set<OutboxEvent>().AddAsync(outboxEvent);
        }

        var result = await _context.SaveChangesAsync();
        await DispatchEventsAsync();

        return result;
    }

    private async Task DispatchEventsAsync()
    {
        var outboxEvents = await _context.Set<OutboxEvent>()
            .Where(e => !e.IsDispatched)
            .ToListAsync();

        foreach (var outboxEvent in outboxEvents)
        {
            var deserializedOutboxEvent = DomainEventSerializer.Deserialize(outboxEvent);
            await _dispatcher.DispatchAsync(deserializedOutboxEvent);

            outboxEvent.IsDispatched = true;
            _context.Set<OutboxEvent>().Update(outboxEvent);
        }

        await _context.SaveChangesAsync();
    }
}
