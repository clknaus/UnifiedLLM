using Application.Interfaces;
using Application.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Interfaces.OpenRouter;

namespace Application.Services;
public class ChatService(IOpenRouterClientService openRouterClientService, IAsyncRepository<Anonymizer> anonymizedChatRequestRepository) : IChatService
{
    public async Task<Result<IChatResponse>> CreateChatCompletionAsync(OpenWebUIChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Stream = false; // TODO enable streaming
        //var entity = new AnonymizedChatRequest()
        //{

        //};
        //var entity = new Anonymizer(request);
        //await anonymizedChatRequestRepository.AddAsync(entity);
        //entity = await anonymizedChatRequestRepository.GetByIdAsync(entity.Id);

        return await openRouterClientService.CreateChatCompletionAsync(request, cancellationToken);

        //return new OpenWebUIChatResponse
        //{
        //    Choices = res.Choices?.Select(choice => new ChatChoice
        //    {
        //        Message = new ChatMessage
        //        {
        //            Role = choice.Message.Role,
        //            Content = choice.Message.Content
        //        },
        //        FinishReason = choice.FinishReason
        //    }).ToList() ?? [],
        //    Id = res.Id,
        //    Model = res.Model,
        //    Created = res.Created
        //};
    }

    public async Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        return await openRouterClientService.GetAvailableModelsAsync(cancellationToken);

        //return new OpenWebUIModelsResponse
        //{
        //    Data = [.. res?.Data?
        //    .Where(model => model != null)
        //    .Select(model => new OpenWebUIModelInfo
        //    {
        //        Id = model.Id,
        //        Object = model.Object,
        //        Created = model.Created,
        //        OwnedBy = model.OwnedBy,
        //    }) ?? []]
        //};
    }
}
