using Core.Interfaces;

namespace Core.Models;
public class Features : IFeatures
{
    public bool ImageGeneration { get; set; }
    public bool CodeInterpreter { get; set; }
    public bool WebSearch { get; set; }
}
