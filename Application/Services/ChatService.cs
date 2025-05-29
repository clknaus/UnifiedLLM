using Core.Interfaces;
using Core.Models;
using Infrastructure.Interfaces;
using UnifiedLLM.Core.Models;

namespace Application.Services;
public class ChatService : IChatService
{
    private readonly ILLMClient _llmClient;
    public ChatService(ILLMClient llmClient)
    {
        _llmClient = llmClient;
    }
    public Task<ChatResponse> CreateChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChatResponse> CreateChatCompletionWithJsonModeAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChatResponse> CreateChatCompletionWithToolsAsync(ChatRequest request, IEnumerable<ToolDefinition> tools, ToolChoice toolChoice = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChatResponse> CreateChatCompletionWithUserAsync(ChatRequest request, string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ModelsResponse> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        return await _llmClient.GetAvailableModelsAsync(cancellationToken);
    }

    public Task<ModelDetails> GetModelDetailsAsync(string modelId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
