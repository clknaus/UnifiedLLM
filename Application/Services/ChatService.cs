using Abstractions;
using Application.Events;
using Application.Handler;
using Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Events;
using Core.Domain.Interfaces;
using Core.General.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces;
using Infrastructure.Interfaces.Providers.OpenRouter;

namespace Application.Services;
public class ChatService(IProviderClientService openRouterClientService, IAsyncRepository<Chat> chatAsyncRepository, IHandlerManagerService handlerManagerService, IAnonymizerService anonymizerService) : IChatService
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
        if (handlerManagerService.DoCommitAsErrorEvent(anonymizedRequestResult)) // TODO see below
            return anonymizedRequestResult.AsResultFailed<IChatRequest, IChatResponse>();

        request = anonymizedRequestResult.Value!;
        var response = await openRouterClientService.TryCreateChatCompletionAsync(request, cancellationToken);
        if (handlerManagerService.DoCommitAsErrorEvent(response))
            return response;

        response = await anonymizerService.Deanonymize(response.Value!);
        if (handlerManagerService.DoCommitAsErrorEvent(response))
            return response;

        var chatResult = new Chat() 
        { 
            ChatRequest = new ChatRequest(request), 
            ChatResponse = new ChatResponse(response.Value!) 
        }.ValidateThenRaiseEvent();

        try
        {
            if (!handlerManagerService.DoCommitAsErrorEvent(chatResult))
                await chatAsyncRepository.AddAsync(chatResult.Value!);

            await handlerManagerService.CommitAsync();
        }
        catch (Exception ex)
        {
            return Result<IChatResponse>.Failure(exception: ex);
        }

        return response;
    }

    public async Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        return await openRouterClientService.TryGetAvailableModelsAsync(cancellationToken);
    }
}
