using Core.Models;

namespace Core.Interfaces;
public interface IChatResponse
{
    public string Id { get; set; }
    public string Object { get; set; } // e.g., "chat.completion"
    public long Created { get; set; } // Unix timestamp
    public string Model { get; set; }
    public IEnumerable<ChatChoice> Choices { get; set; }
    //public IUsage Usage { get; set; } // Token usage statistics
    public string SystemFingerprint { get; set; } // Optional
}
