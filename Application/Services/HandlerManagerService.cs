using Application.Events;
using Application.Handler;
using Core.Domain.Interfaces;
using Core.General.Interfaces;
using Core.General.Models;

namespace Application.Services;
public class HandlerManagerService(IUnitOfWork unitOfWork, IDomainEventQueue domainEventQueue) : IHandlerManagerService
{
    public Task CommitAsync() => unitOfWork.CommitAsync();

    public bool DoCommitAsErrorEvent<T>(Result<T> result, ErrorType errorType = default)
    {
        if (result.IsSuccess)
        {
            return false;
        }

        domainEventQueue.Enqueue(new LogEvent() { Message = result.ErrorMessage, ErrorType = errorType });
        unitOfWork.CommitAsync();

        return true;
    }
}
