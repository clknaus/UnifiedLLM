using Core.Domain.Entities;
using Core.Domain.Models;
using System.Collections.Generic;

namespace Core.Domain.Interfaces;
public interface IChatResponse
{
    public string Id { get; set; }
    public string Object { get; set; } // e.g., "chat.completion"
    public long Created { get; set; } // Unix timestamp
    public string Model { get; set; }
    public IReadOnlyList<ChatChoice> Choices { get; set; }
    //public IUsage Usage { get; set; } // Token usage statistics
    public string SystemFingerprint { get; set; } // Optional
}
