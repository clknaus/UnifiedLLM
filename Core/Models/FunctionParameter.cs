using Core.Interfaces;

namespace Core.Models;
public class FunctionParameter : IFunctionParameter
{
    public string Type { get; set; }
    public string Description { get; set; }
    public IDictionary<string, FunctionParameter> Properties { get; set; }
    public IList<string> Required { get; set; }
}