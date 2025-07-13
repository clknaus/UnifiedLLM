using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class Features : IFeatures
{
    public bool ImageGeneration { get; set; }
    public bool CodeInterpreter { get; set; }
    public bool WebSearch { get; set; }
}
