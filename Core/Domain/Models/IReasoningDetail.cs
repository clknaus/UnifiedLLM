using System.Text.Json.Serialization;

namespace Core.Domain.Models;

public interface IReasoningDetail
{
    public string? Type { get; set; }

    public string? Text { get; set; }

    public string? Format { get; set; }

    public int? Index { get; set; }
}