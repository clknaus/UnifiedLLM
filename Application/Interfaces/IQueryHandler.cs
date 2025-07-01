using Core.Models;

namespace Application.Interfaces;
public interface IQueryHandler<TOut>
{
    Task<Result<TOut>> HandleAsync(CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TInput, TOutput>
{
    Task<Result<TOutput>> HandleAsync(TInput obj, CancellationToken cancellationToken = default);
}