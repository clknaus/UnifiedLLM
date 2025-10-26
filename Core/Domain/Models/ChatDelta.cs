using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.General.Models;

namespace Core.Domain.Models;
public class ChatDelta : Entity<Guid>, IChatDelta 
{
    public string? Role { get; set; }
    public string? Content { get; set; }
    public string? Reasoning { get; set; }
    public IReadOnlyList<ReasoningDetail>? ReasoningDetails { get; set; }
}