using Application.Events;
using Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Events;
using Core.Domain.Interfaces;
using Core.General.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces;
using Infrastructure.Interfaces.Providers.OpenRouter;

namespace Application.Services;
public class ChatService(IProviderClientService openRouterClientService, IAsyncRepository<Chat> chatAsyncRepository, IUnitOfWork unitOfWork, IDomainEventQueue domainEventQueue) : IChatService
{
    public async Task<Result<IChatResponse>> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Stream = false; // TODO enable streaming

        // create or get chat entity along with a guid


        // keep track of the saved entity by guid
        // get or hash text to keep track of chat entity
        // may use cosinus properties to check on text similarity for tracking

        // use factory to create Anonymizer (inside Chat entity?)
        // use Search along with ISearchService call
        // use Functor as object to command a call (research cosine similarity behavior for getting the right function)
        // call Functor along with IOpenRouteClientService to determine function so that semantics are sure.
        // parameterize preferred model for every call in IOpenRouteClientService
        var response = await openRouterClientService.CreateChatCompletionAsync(request, cancellationToken);
        if (response.IsFailure)
        {
            domainEventQueue.Enqueue(new ErrorLogEvent(response.Error));
            await unitOfWork.CommitAsync();
            return response;
        }

        var chat = new Chat()
        {
            ChatRequest = new ChatRequest(request),
            ChatResponse = new ChatResponse(response.Value!)
        };

        var validatedChatResult = chat.ValidateThenRaiseEvent();
        if (validatedChatResult.IsFailure)
        {
            domainEventQueue.Enqueue(new ErrorLogEvent(validatedChatResult.Error));
        }
        else
        {
            await chatAsyncRepository.AddAsync(validatedChatResult.Value!);
        }

        await unitOfWork.CommitAsync();
        return response;

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
