namespace Core.Domain.Interfaces;

public interface IModelInfo
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string OwnedBy { get; set; }
}
