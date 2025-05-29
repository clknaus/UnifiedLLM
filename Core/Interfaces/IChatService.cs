using Core.Models;
using UnifiedLLM.Core.Models;

namespace Core.Interfaces;
public interface IChatService
{
    Task<ChatResponse> CreateChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default);
    Task<ModelsResponse> GetAvailableModelsAsync(CancellationToken cancellationToken = default);
    Task<ModelDetails> GetModelDetailsAsync(string modelId, CancellationToken cancellationToken = default);
    Task<ChatResponse> CreateChatCompletionWithToolsAsync(ChatRequest request, IEnumerable<ToolDefinition> tools, ToolChoice toolChoice = null, CancellationToken cancellationToken = default);
    Task<ChatResponse> CreateChatCompletionWithUserAsync(ChatRequest request, string userId, CancellationToken cancellationToken = default);
    Task<ChatResponse> CreateChatCompletionWithJsonModeAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
