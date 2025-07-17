using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.General.Models;

namespace Core.Domain.Entities;
public class ChatRequest : Entity<Guid>, IChatRequest
{
    public string? Id { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public bool Stream { get; set; }
}