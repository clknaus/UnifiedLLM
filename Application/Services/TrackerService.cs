using Core;
using Core.General.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces.Tracker;

namespace Application.Services;
public class TrackerService<TId>(IHashHandler hashHandler) : ITrackerService<TId> where TId : new()
{
    public bool IsTracked(Entity<TId> entity, ITrackerService<TId>.Algorithm strategy)
    {
        if (entity is null)
            return false;

        switch (strategy)
        {
            case ITrackerService<TId>.Algorithm.Id:
                return entity.Id != null;

            case ITrackerService<TId>.Algorithm.Hash:
                return hashHandler.IsHash(entity.GetHashCode().ToString());

            case ITrackerService<TId>.Algorithm.StringComparision:
                // TODO
                return false;

            case ITrackerService<TId>.Algorithm.ContentSimilarity:
                return false;

            case ITrackerService<TId>.Algorithm.CosineSimilarity:
                return false;

            default:
                // TrackerAlgorithm.Default
                // chain / builer pattern
                return IsTracked(entity);
        }
    }

    public bool IsTracked(Entity<TId> entity)
    {
        if (entity == null)
            return false;

        return IsTracked(entity, strategy: ITrackerService<TId>.Algorithm.Id)
            || IsTracked(entity, strategy: ITrackerService<TId>.Algorithm.Hash)
            || IsTracked(entity, strategy: ITrackerService<TId>.Algorithm.StringComparision)
            || IsTracked(entity, strategy: ITrackerService<TId>.Algorithm.ContentSimilarity)
            || IsTracked(entity, strategy: ITrackerService<TId>.Algorithm.CosineSimilarity);
    }
}
