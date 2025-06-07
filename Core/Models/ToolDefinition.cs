using Core.Interfaces;

namespace Core.Models;
public class ToolDefinition : IToolDefinition
{
    public FunctionDefinition Function { get; set; }
}
