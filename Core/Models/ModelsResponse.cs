using Core.Interfaces;

namespace Core.Models;

public class ModelsResponse : IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
