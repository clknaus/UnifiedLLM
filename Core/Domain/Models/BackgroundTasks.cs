using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class BackgroundTasks : IBackgroundTasks
{
    public bool TitleGeneration { get; set; }
    public bool TagsGeneration { get; set; }
}
