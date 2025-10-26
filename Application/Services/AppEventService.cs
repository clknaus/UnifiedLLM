using Application.Events;
using Application.Handler;
using Core.Domain.Interfaces;
using Core.General.Interfaces;
using Core.General.Models;

namespace Application.Services;
public class AppEventService(IUnitOfWork unitOfWork, IDomainEventQueue domainEventQueue) : IAppEventService
{
    public Task CommitAsync() => unitOfWork.CommitAsync();

    public bool HandleError<T>(Result<T> result, ErrorType errorType = default)
    {
        if (result?.IsSuccess == true)
        {
            return false;
        }

        EnqueueAndCommit(result!);

        return true;
    }

    public async Task<bool> HandleError<T>(IAsyncEnumerable<Result<T>> result, ErrorType errorType = default)
    {
        var enumerator = result.GetAsyncEnumerator();
        if (enumerator.Current.IsSuccess)
        {
            return true;
        }

        EnqueueAndCommit(enumerator.Current);

        return false;
    }

    public Result<T> HandleException<T>(Exception ex, ErrorType errorType = default, string? message = default)
    {
        EnqueueAndCommit<T>(Result<T>.Failure(exception: ex, errorType: errorType));
        return Result<T>.Failure(message ?? "An error has occured.", errorType: errorType);
    }

    private void EnqueueAndCommit<T>(Result<T> result, ErrorType errorType = default)
    {
        domainEventQueue.Enqueue(new LogEvent() { Message = result!.Exception?.Message ?? result.ErrorMessage, ErrorType = errorType });
        unitOfWork.CommitAsync();
    }
}
