using Core.Domain.Interfaces;
using Core.General.Models;

namespace Infrastructure.Interfaces.Providers.OpenRouter;
public interface IProviderClientService
{
    Task<Result<IChatResponse>> TryCreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default);
    Task<Result<IModelsResponse>> TryGetAvailableModelsAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<Result<IChatResponse>> TryStreamChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default);
}
