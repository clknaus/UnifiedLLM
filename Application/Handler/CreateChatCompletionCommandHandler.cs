using Application.Interfaces;
using Application.Services;
using Core.Domain.Interfaces;
using Core.General.Models;
using System.Threading.Tasks;

namespace Application.Handler;

public class CreateChatCompletionCommandHandler(IChatService chatService) : ICommandHandler<IChatRequest, IChatResponse>
{
    public async Task<Result<IChatResponse>> HandleAsync(IChatRequest request, CancellationToken? cancellationToken = default)
    {
        // boundary validation

        if (request is null)
            return Result<IChatResponse>.Failure(message: "null object", errorType: ErrorType.Validation);


        // model validation

        if (request.Model == null)
            return Result<IChatResponse>.Failure("Model is required", errorType: ErrorType.Validation);

        if (!request.Messages?.Any() ?? false)
            return Result<IChatResponse>.Failure("Message is required", errorType: ErrorType.Validation);

        try
        {
            return await chatService.CreateChatCompletionAsync(request);
        }
        catch (Exception)
        {
            return Result<IChatResponse>.Failure(ErrorType.Unknown);
        }
    }

    public async IAsyncEnumerable<Result<IChatResponse>> HandleStreamAsync(IChatRequest request, CancellationToken? cancellationToken = null)
    {
        // boundary validation

        if (request is null)
            yield return Result<IChatResponse>.Failure(message: "null object", errorType: ErrorType.Validation);


        // model validation

        if (request!.Model == null)
            yield return Result<IChatResponse>.Failure("Model is required", errorType: ErrorType.Validation);

        if (!request.Messages?.Any() ?? false)
            yield return Result<IChatResponse>.Failure("Message is required", errorType: ErrorType.Validation);

        await foreach (var chunkResult in chatService.CreateChatCompletionStreamAsync(request))
        {
            if (chunkResult.IsFailure)
            {
                // do stuff
            }

            yield return chunkResult;
        }
    }
}
