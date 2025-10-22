using Abstractions;
using Abstractions.Extension;
using Application.Handler;
using Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces;
using Infrastructure.Interfaces.Providers.OpenRouter;

namespace Application.Services;
public class ChatService(IProviderClientService openRouterClientService, IAsyncRepository<Chat> chatAsyncRepository, IAppEventService appEventService, IAnonymizerService anonymizerService) : IChatService
{
    public async Task<Result<IChatResponse>> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Stream = false; // TODO enable streaming

        var anonymizedRequestResult = await anonymizerService.Anonymize(request);
        if (appEventService.HandleError(anonymizedRequestResult)) // TODO see below
            return anonymizedRequestResult.AsResultFailed<IChatRequest, IChatResponse>();

        var chat = new Chat() { ChatRequest = new ChatRequest(request) };

        request = anonymizedRequestResult.Value!;
        var response = await openRouterClientService.TryCreateChatCompletionAsync(request, cancellationToken);
        if (appEventService.HandleError(response))
            return response;

        response = await anonymizerService.Deanonymize(response.Value!);
        if (appEventService.HandleError(response))
            return response;

        try
        {
            chat.ChatResponse = new ChatResponse(response?.Value!);
            if (!appEventService.HandleError(chat.ValidateThenRaiseEvent()))
                await chatAsyncRepository.AddAsync(chat);

            await appEventService.CommitAsync();
        }
        catch (Exception ex)
        {
            return appEventService.HandleException<IChatResponse>(ex);
        }

        return response!;
    }

    public async Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        return await openRouterClientService.TryGetAvailableModelsAsync(cancellationToken);
    }
}
