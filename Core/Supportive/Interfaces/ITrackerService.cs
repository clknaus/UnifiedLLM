using Core.Domain.Entities;

namespace Application.Services;
public interface ITrackerService
{
    public bool IsTracked(Tracker tracker, TrackerAlgorithm strategy);
    public bool IsTracked(Tracker tracker);
    public enum TrackerAlgorithm
    {
        Default = 0,
        Id = 1,
        Hash = 2,
        StringComparision = 3,
        ContentSimilarity = 4,
        CosineSimilarity = 5
    }
}