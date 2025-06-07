using Core.Interfaces;

namespace Core.Models;
public class BackgroundTasks : IBackgroundTasks
{
    public bool TitleGeneration { get; set; }
    public bool TagsGeneration { get; set; }
}
