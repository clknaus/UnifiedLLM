namespace Core.Supportive.Interfaces;

public interface IAsyncRepository<T> where T : class, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> ListAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> GetPagedResponseAsync(int page, int size);
}
