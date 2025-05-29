namespace Core.Models;
public class ToolChoice
{
    public string Type { get; set; } = "function"; // Currently, only "function" is supported
    public FunctionCall Function { get; set; }
}
