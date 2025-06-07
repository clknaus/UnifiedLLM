using Core.Interfaces;

namespace Infrastructure.Models.OpenRouter;
public class OpenRouterModelInfo : IModelInfo
{
    public string Id { get; set; }
    public string Object { get; set; } = "model";
    public long Created { get; set; } = 0;
    public string OwnedBy { get; set; } = "ollama";
}
