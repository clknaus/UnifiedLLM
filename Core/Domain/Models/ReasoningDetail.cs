using Core.General.Models;
namespace Core.Domain.Models;
public class ReasoningDetail : Entity<Guid>, IReasoningDetail
{
    public string? Type { get; set; }
    public string? Text { get; set; }
    public string? Format { get; set; }
    public int? Index { get; set; }
}