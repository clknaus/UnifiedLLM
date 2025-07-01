using Core.Models;

namespace Application.Interfaces;

public interface ICommandHandler<TOut>
{
    Task<Result<TOut>> HandleAsync(CancellationToken? cancellationToken = default);
}

public interface ICommandHandler<TIn, TOut>
{
    Task<Result<TOut>> HandleAsync(TIn obj, CancellationToken? cancellationToken = default);
}