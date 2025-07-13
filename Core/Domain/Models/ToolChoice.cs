using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class ToolChoice : IToolChoice
{
    public string Type { get; set; }
    public FunctionCall Function { get; set; }
}
