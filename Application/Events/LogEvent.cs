using Core.Domain.Events;
using Core.General.Models;
using Microsoft.Extensions.Logging;

namespace Application.Events;
public class LogEvent : DomainEventBase
{
    public required string Message { get; init; }
    public LogLevel LogLevel { get; init; }
    public ErrorType ErrorType { get; init; }
}
