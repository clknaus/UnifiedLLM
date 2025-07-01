using Application.Interfaces;
using Core;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Interfaces.OpenRouter;

namespace Application.Services;
public class ChatService(IOpenRouterClientService openRouterClientService, IAsyncRepository<Chat> chatAsyncRepository, IUnitOfWork unitOfWork) : IChatService
{
    public async Task<Result<IChatResponse>> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Stream = false; // TODO enable streaming

        // create or get chat entity along with a guid
        var chat = new Chat();
        chat.ValidateThenRaiseEvent();
        Chat entity = await chatAsyncRepository.AddAsync(chat);
        await unitOfWork.CommitAsync();

        // keep track of the saved entity by guid
        // get or hash text to keep track of chat entity
        // may use cosinus properties to check on text similarity for tracking

        // use factory to create Anonymizer (inside Chat entity?)
        // use Search along with ISearchService call
        // use Functor as object to command a call (research cosine similarity behavior for getting the right function)
        // call Functor along with IOpenRouteClientService to determine function so that semantics are sure.
        // parameterize preferred model for every call in IOpenRouteClientService

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
    }
}
