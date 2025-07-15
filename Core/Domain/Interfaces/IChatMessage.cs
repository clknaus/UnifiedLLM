namespace Core.Domain.Interfaces;
public interface IChatMessage
{
    string Role { get; set; }
    string Content { get; set; }
}