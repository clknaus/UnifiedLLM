using Core.Interfaces;

namespace Core.Models;
public class ChatChoice : Entity<Guid>, IChatChoice
{
    public int Index { get; set; }
    public ChatMessage Message { get; set; }
    public string FinishReason { get; set; }

}
