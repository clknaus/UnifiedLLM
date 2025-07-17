using Core.General.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;

namespace Infrastructure.Persistence.Repositories;
public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IDomainEventDispatcher _dispatcher;

    public EfUnitOfWork(AppDbContext context, IDomainEventDispatcher dispatcher)
    {
        _context = context;
        _dispatcher = dispatcher;
    }

    public async Task<int> CommitAsync()
    {
        var result = await _context.SaveChangesAsync();

        var aggregates = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .ToList();

#if debug
        Console.WriteLine("try _context.ChangeTracker.Entries().ToList();, result:
        var tracked = _context.ChangeTracker.Entries().ToList();
        Console.WriteLine($"Tracked entities: {tracked.Count}");
        foreach (var entry in tracked)
        {
            Console.WriteLine(entry.Entity.GetType().Name);
        }
#endif

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.GetDomainEvents())
            {
                await _dispatcher.DispatchAsync(domainEvent);
            }

            aggregate.ClearDomainEvents();
        }

        return result;
    }
}
