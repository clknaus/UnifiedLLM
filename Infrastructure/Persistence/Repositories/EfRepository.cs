using Core.Domain.Entities;
using Core.General.Interfaces;
using Core.Supportive.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class EfRepository<T> : IAsyncRepository<T> where T : class, IAggregateRootGeneric<T>
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> ListAllAsync()
    {
        return await _dbSet.ToListAsync() ?? [];
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SoftDeleteAsync(T entity)
    {
        if (entity is not BaseEntity baseEntity)
            return;

        baseEntity.IsDeleted = true;
        await UpdateAsync(entity);
    }

    public async Task<IEnumerable<T>> GetPagedResponseAsync(int page, int size)
    {
        return await _dbSet.Skip((page - 1) * size).Take(size).ToListAsync();
    }

}

