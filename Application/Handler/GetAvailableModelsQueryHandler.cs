using Application.Interfaces;
using Core.Interfaces;
using Core.Models;

namespace Application.Handler;

public class GetAvailableModelsQueryHandler(IChatService chatService) : IQueryHandler<IModelsResponse>
{
    public async Task<Result<IModelsResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await chatService.GetAvailableModelsAsync(cancellationToken);
    }
}
