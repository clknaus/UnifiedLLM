using Core.Domain.Interfaces;
using Core.General.Models;
using Infrastructure.Models.OpenRouter;
using System.Runtime.CompilerServices;

namespace Infrastructure.Interfaces.OpenRouter;
public interface IOpenRouterClientService
{
    Task<Result<IChatResponse>> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default);
    Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<OpenRouterChatStreamResponse?> GetChatCompletionStreamAsync(IChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    //IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
