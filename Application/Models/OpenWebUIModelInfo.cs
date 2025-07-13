using Core.Domain.Interfaces;

namespace Application.Models;
public class OpenWebUIModelInfo : IModelInfo
{
    public string Id { get; set; }
    public string Object { get; set; } = "model";
    public long Created { get; set; } = 0;
    public string OwnedBy { get; set; } = "ollama";
}
