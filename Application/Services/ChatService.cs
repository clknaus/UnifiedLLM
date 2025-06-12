using Application.Interfaces;
using Application.Models;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Interfaces.OpenRouter;

namespace Application.Services;
public class ChatService(IOpenRouterClientService openRouterClientService, IAsyncRepository<AnonymizedChatRequest> anonymizedChatRequestRepository) : IChatService
{
    public async Task<IChatResponse?> CreateChatCompletionAsync(OpenWebUIChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Stream = false; // TODO enable streaming
        //var entity = new AnonymizedChatRequest()
        //{

        //};
        var entity = new AnonymizedChatRequest();
        await anonymizedChatRequestRepository.AddAsync(entity);
        entity = await anonymizedChatRequestRepository.GetByIdAsync(entity.Id);

        var res = await openRouterClientService.CreateChatCompletionAsync(request, cancellationToken);
        if (res == null)
            return null;

        return res;

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

    public async Task<IModelsResponse?> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        var res = await openRouterClientService.GetAvailableModelsAsync(cancellationToken);
        if (res == null) 
            return null;

        return res;

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
