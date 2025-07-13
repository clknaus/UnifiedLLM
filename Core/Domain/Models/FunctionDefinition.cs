using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class FunctionDefinition : IFunctionDefinition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IDictionary<string, FunctionParameter> Parameters { get; set; }
}
