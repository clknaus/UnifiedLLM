using Core.Domain.Models;

namespace Core.Domain.Interfaces
{
    public interface IChatDelta
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
        public string? Reasoning { get; set; }
        public IReadOnlyList<ReasoningDetail>? ReasoningDetails { get; set; }
    }
}