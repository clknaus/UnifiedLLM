namespace Core.Models;
public class ToolDefinition
{
    public string Type { get; set; } = "function"; // Currently, only "function" is supported
    public FunctionDefinition Function { get; set; }
}
