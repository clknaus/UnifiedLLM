using Core;

namespace Application.Services;
public interface ITrackerService<TId> where TId : new()
{
    public bool IsTracked(Entity<TId> entity, TrackerAlgorithm strategy);
    public bool IsTracked(Entity<TId> entity);
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