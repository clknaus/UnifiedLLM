using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class ChatMessage : Entity<Guid>, IChatMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}
