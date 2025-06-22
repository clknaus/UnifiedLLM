using Application.Models;
using Core.Interfaces;
using Core.Models;

namespace Application.Interfaces;
public interface IChatService
{
    Task<Result<IChatResponse>> CreateChatCompletionAsync(OpenWebUIChatRequest request, CancellationToken cancellationToken = default);
    Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default);
    //IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
    //Task<ModelDetails> GetModelDetailsAsync(string modelId, CancellationToken cancellationToken = default);
    //Task<ChatResponse> CreateChatCompletionWithToolsAsync(ChatRequest request, IEnumerable<ToolDefinition> tools, ToolChoice toolChoice = null, CancellationToken cancellationToken = default);
    //Task<ChatResponse> CreateChatCompletionWithUserAsync(ChatRequest request, string userId, CancellationToken cancellationToken = default);
    //Task<ChatResponse> CreateChatCompletionWithJsonModeAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
