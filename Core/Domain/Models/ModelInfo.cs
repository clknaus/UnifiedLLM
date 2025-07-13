using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class ModelInfo : IModelInfo
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string OwnedBy { get; set; }
}
