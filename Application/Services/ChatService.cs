using Abstractions;
using Abstractions.Extension;
using Application.Handler;
using Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces;
using Infrastructure.Interfaces.Providers.OpenRouter;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

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

    public async IAsyncEnumerable<Result<IChatResponse>> CreateChatCompletionStreamAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        var anonymizedRequestResult = await anonymizerService.Anonymize(request);
        if (appEventService.HandleError(anonymizedRequestResult))
            yield return anonymizedRequestResult.AsResultFailed<IChatRequest, IChatResponse>();

        request = anonymizedRequestResult.Value!;
        var sb = new StringBuilder();
        IChatResponse? response = null;

        await foreach (var chunkResult in openRouterClientService.TryStreamChatCompletionAsync(request, cancellationToken))
        {
            // boundary check
            if (appEventService.HandleError(chunkResult))
                yield return chunkResult;

            response ??= chunkResult.Value!;

            // integrity check
            if (!IsSameChat(response, chunkResult.Value!) || chunkResult?.Value?.Choices is { Count: 0 } choices)
                yield return Result<IChatResponse>.Failure("Integrity failed.");

            // prepare entity
            sb.Append(chunkResult?.Value?.Choices[0].Delta.Content);

            yield return chunkResult!;
        }

        try
        {
            response!.Choices[0]?.Delta.Content = sb.ToString();

            var chat = new Chat
            {
                ChatRequest = new ChatRequest(request),
                ChatResponse = new ChatResponse(response)
            };

            if (!appEventService.HandleError(chat.ValidateThenRaiseEvent()))
                await chatAsyncRepository.AddAsync(chat); // OR UPDATE !!!

            await appEventService.CommitAsync();
        }
        catch (Exception ex)
        {
            appEventService.HandleException<IChatResponse>(ex);
        }
    }

    public async Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        return await openRouterClientService.TryGetAvailableModelsAsync(cancellationToken);
    }

    private bool IsSameChat(IChatResponse chatResponse, IChatResponse compare) => chatResponse?.Id.Equals(compare?.Id, StringComparison.OrdinalIgnoreCase) ?? false;
}
