using Core;

namespace Core.Supportive.Interfaces.Tracker;
public interface ITrackerService<TId> where TId : new()
{
    public bool IsTracked(Entity<TId> entity, Algorithm strategy);
    public bool IsTracked(Entity<TId> entity);
    public enum Algorithm
    {
        Default = 0,
        Id = 1,
        Hash = 2,
        StringComparision = 3,
        ContentSimilarity = 4,
        CosineSimilarity = 5
    }
}