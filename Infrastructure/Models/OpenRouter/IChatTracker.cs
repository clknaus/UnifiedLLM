namespace Infrastructure.Models.OpenRouter;

public interface IChatTracker
{
    public string? SessionId { get; set; }
    public string? ChatId { get; set; }
    public string? Id { get; set; }
}