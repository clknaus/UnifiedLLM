using Application.Events;
using Core.General.Models;
using Core.Supportive.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Handler;

public class LogEventHandler(ILogger<LogEvent> logger) : AsyncDomainEventHandler<LogEvent>
{
    public override async Task<Result<HandlerResult>> HandleAsync(LogEvent domainEvent)
    {
        var message = $"{domainEvent.Message}, GUID: {domainEvent.EventId}";
        if (domainEvent.ErrorType != ErrorType.Unknown)
        {
            message = $"Error Type {domainEvent.ErrorType}: " + message;
        }

        switch (domainEvent.LogLevel)
        {
            case LogLevel.Error:
                logger.LogError(message);
                break;

            case LogLevel.Warning:
                logger.LogWarning(message);
                break;

            case LogLevel.Information:
                logger.LogInformation(message);
                break;

            case LogLevel.Debug:
                logger.LogDebug(message);
                break;

            case LogLevel.Critical:
                logger.LogCritical(message);
                break;

            case LogLevel.None:
                break;
        }

        return Success;
    }
}