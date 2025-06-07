using Core.Interfaces;

namespace Core.Models;
public class ToolChoice : IToolChoice
{
    public string Type { get; set; }
    public FunctionCall Function { get; set; }
}
