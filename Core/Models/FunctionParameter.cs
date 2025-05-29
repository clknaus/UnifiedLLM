namespace Core.Models;

public class FunctionParameter
{
    public string Type { get; set; } // e.g., "string", "number", "object"
    public string Description { get; set; }
    public IDictionary<string, FunctionParameter> Properties { get; set; } // For nested objects
    public IList<string> Required { get; set; } // List of required parameter names
}
