using Core.Interfaces;
using Core.Models;

namespace Infrastructure.Models.OpenRouter;
public class OpenRouterModelsResponse : IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
