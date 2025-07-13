using Core.Domain.Interfaces;

namespace Core.Domain.Models;

public class ModelsResponse : IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
