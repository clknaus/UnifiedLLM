using Abstractions;
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
public class ChatService(IProviderClientService openRouterClientService, IAsyncRepository<Chat> chatAsyncRepository, IUnitOfWork unitOfWork, IDomainEventQueue domainEventQueue, IAnonymizerService anonymizerService) : IChatService
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

        var anonymizedRequestResult = await anonymizerService.Anonymize(request);
        if (anonymizedRequestResult.IsFailure) // TODO see below
            return anonymizedRequestResult.AsResultFailed<IChatRequest, IChatResponse>();
        request = anonymizedRequestResult.Value!;

        var response = await openRouterClientService.CreateChatCompletionAsync(request, cancellationToken);
        if (response.IsFailure)
        {
            // TODO add ErrorManager that does: capture events, logs, sends e-mail, does other stuff
            domainEventQueue.Enqueue(new ErrorLogEvent(response.ErrorMessage)); // TODO abstract into CommitAsync
            await unitOfWork.CommitAsync(); // TODO add Result object directly
            return response;
        }

        response = await anonymizerService.Deanonymize(response.Value!);

        var chatResult = new Chat()
        {
            ChatRequest = new ChatRequest(request),
            ChatResponse = new ChatResponse(response.Value!)
        }.ValidateThenRaiseEvent();

        if (chatResult.IsFailure)
        {
            domainEventQueue.Enqueue(new ErrorLogEvent(chatResult.ErrorMessage));
        }
        else
        {
            await chatAsyncRepository.AddAsync(chatResult.Value!);
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
