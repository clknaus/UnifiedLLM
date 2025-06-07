using Core.Models;

namespace Core.Interfaces;

public interface IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
