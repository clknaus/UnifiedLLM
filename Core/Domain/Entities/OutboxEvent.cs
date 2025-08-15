using Core.General.Models;
using Core.Supportive.Interfaces.DomainEvents;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class OutboxEvent : Entity<Guid> , IDomainEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public int Version { get; set; }
    [NotMapped]
    public IReadOnlyDictionary<string, object> Metadata { get; set; }
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
    public bool IsDispatched { get; set; } = false;
}

