using Abstractions;
using Application.Interfaces;
using Application.Models;
using Core.Models;
using Infrastructure.Interfaces;

namespace Application.Services;
public class ChatApplicationService(ILLMClient llmClient) : IChatApplicationService
{
    public Task<OpenWebUIChatResponse> CreateChatCompletionAsync(OpenWebUIChatRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<OpenWebUIModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        var res = await llmClient.GetAvailableModelsAsync(cancellationToken);

        if (!res.IsSuccess)
            return res.AsResultFailed<OpenWebUIModelsResponse>();

        return new OpenWebUIModelsResponse
        {
            Data = [.. res.Value?.Data?
            .Where(model => model != null)
            .Select(model => new OpenWebUIModelInfo
            {
                Id = model.Id,
                Object = model.Object,
                Created = model.Created,
                OwnedBy = model.OwnedBy,
            }) ?? []]
        }.AsResultSuccess();
    }
}
