using Core.Domain.Events;

namespace Application.Events;

public class ErrorLogEvent : DomainEventBase
{
    public string Error { get; set; }

    public ErrorLogEvent() { }
    public ErrorLogEvent(string error)
    {
        Error = error;
    }
}