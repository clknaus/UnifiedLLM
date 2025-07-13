using Abstractions;
using Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Extensions;
using Core.General.Models;

namespace Application.Handler;
public record CreateChatCompletionCommand(IChatRequest Request);

public class CreateChatCompletionCommandHandler(IChatService chatService) : ICommandHandler<IChatRequest, IChatResponse>
{
    public async Task<Result<IChatResponse>> HandleAsync(IChatRequest request, CancellationToken? cancellationToken = default)
    {
        // boundary validation

        if (request is null)
            return Result<IChatResponse>.Failure("null object", ErrorType.Validation);


        // model validation

        if (request.Model == null)
            return Result<IChatResponse>.Failure("Model is required", ErrorType.Validation);

        if (!request.Messages?.Any() ?? false)
            return Result<IChatResponse>.Failure("Message is required", ErrorType.Validation);

        try
        {
            return await chatService.CreateChatCompletionAsync(request);
        }
        catch (Exception)
        {
            // TODO log
            return Result<IChatResponse>.Failure(ErrorType.NotFound);
        }
    }
}
