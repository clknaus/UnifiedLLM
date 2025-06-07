using Core.Interfaces;

namespace Core.Models;
public class ChatMessage : IChatMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}
