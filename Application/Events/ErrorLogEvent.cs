using Core.Domain.Events;

namespace Application.Events;

public class ErrorLogEvent : DomainEventBase
{
    public readonly string Error;

    public ErrorLogEvent(string error)
    {
        this.Error = error;
    }
}