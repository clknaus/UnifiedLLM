using Core.Domain.Interfaces;
using Core.General.Models;
using Infrastructure.Models.OpenRouter;
using System.Runtime.CompilerServices;

namespace Infrastructure.Interfaces.Providers.OpenRouter;
public interface IProviderClientService
{
    Task<Result<IChatResponse>> TryCreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default);
    Task<Result<IModelsResponse>> TryGetAvailableModelsAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<OpenRouterChatStreamResponse?> GetChatCompletionStreamAsync(IChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    //IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
