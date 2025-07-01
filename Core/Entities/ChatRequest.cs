using Core.Interfaces;
using Core.Models;

namespace Core.Entities;
public class ChatRequest : Entity<Guid>, IChatRequest
{
    public string? Id { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public bool Stream { get; set; }
}