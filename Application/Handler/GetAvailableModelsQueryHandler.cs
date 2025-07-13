using Application.Interfaces;
using Core.Domain.Interfaces;
using Core.General.Models;

namespace Application.Handler;

public class GetAvailableModelsQueryHandler(IChatService chatService) : IQueryHandler<IModelsResponse>
{
    public async Task<Result<IModelsResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await chatService.GetAvailableModelsAsync(cancellationToken);
    }
}
