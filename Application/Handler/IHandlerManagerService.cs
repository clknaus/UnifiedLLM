using Core.General.Models;

namespace Application.Handler;
public interface IHandlerManagerService
{
    bool DoCommitAsErrorEvent<T>(Result<T> result, ErrorType errorType = default);
    public Task CommitAsync();
}
