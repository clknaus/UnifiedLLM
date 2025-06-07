using Core.Interfaces;

namespace Core.Models;
public class FunctionDefinition : IFunctionDefinition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IDictionary<string, FunctionParameter> Parameters { get; set; }
}
