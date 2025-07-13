using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class ToolDefinition : IToolDefinition
{
    public FunctionDefinition Function { get; set; }
}
