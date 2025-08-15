using Core.Domain.Entities;
using Core.Supportive.Interfaces.DomainEvents;
using System.Text.Json;

namespace Infrastructure.Persistence;

public static class DomainEventSerializer
{
    public static OutboxEvent ToOutboxEvent(IDomainEvent domainEvent)
    {
        var payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
        return new OutboxEvent
        {
            Type = domainEvent.GetType().AssemblyQualifiedName!,
            Payload = payload
        };
    }

    public static IDomainEvent Deserialize(OutboxEvent outboxEvent)
    {
        var type = Type.GetType(outboxEvent.Type)!;
        return (IDomainEvent)JsonSerializer.Deserialize(outboxEvent.Payload, type)!;
    }
}

