namespace Core.Supportive.Interfaces.Tracker;

public interface IChatTracker
{
    public string? SessionId { get; set; }
    public string? ChatId { get; set; }
    public string? Id { get; set; }
}