using Core.Interfaces;
using Infrastructure.Models.OpenRouter;
using System.Runtime.CompilerServices;

namespace Infrastructure.Interfaces.OpenRouter;
public interface IOpenRouterClientService
{
    Task<IChatResponse?> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default);
    Task<IModelsResponse?> GetAvailableModelsAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<OpenRouterChatStreamResponse?> GetChatCompletionStreamAsync(IChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    //IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
