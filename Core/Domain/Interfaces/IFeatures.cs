namespace Core.Domain.Interfaces;

public interface IFeatures
{
    bool ImageGeneration { get; set; }
    bool CodeInterpreter { get; set; }
    bool WebSearch { get; set; }
}