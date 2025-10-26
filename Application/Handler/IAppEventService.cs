using Core.General.Models;

namespace Application.Handler;
public interface IAppEventService
{
    bool HandleError<T>(Result<T> result, ErrorType errorType = default);
    Task<bool> HandleError<T>(IAsyncEnumerable<Result<T>> result, ErrorType errorType = default);
    Result<T> HandleException<T>(Exception ex, ErrorType errorType = default, string? message = default);
    public Task CommitAsync();
}
