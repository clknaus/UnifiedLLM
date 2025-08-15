using Core.Domain.Models;

namespace Core.Domain.Interfaces;
public interface IChatChoice
{
    public int Index { get; set; }
    public ChatMessage Message { get; set; }
    public string? FinishReason { get; set; }
}
