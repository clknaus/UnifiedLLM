namespace Core.Domain.Events;
public class AnonymizerErrorEvent(Guid anonymizerId, string error) : DomainEventBase
{
    public Guid AnonymizerId { get; } = anonymizerId;
    public string Error { get; } = error;
}
