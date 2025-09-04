using Application.Events;
using Application.Handler;
using Core.General.Models;
using Core.Supportive.Enums;

namespace API;
public class ErrorLogEventHandler(ILogger<ErrorLogEventHandler> logger) : AsyncDomainEventHandler<ErrorLogEvent>
{
    public override async Task<Result<HandlerResult>> HandleAsync(ErrorLogEvent domainEvent)
    {
        logger.LogError(domainEvent.Error);
        return Success;
    }
}