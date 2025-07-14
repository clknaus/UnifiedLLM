using Core;
using Core.General.Interfaces;

namespace Application.Services;
public class TrackerService<TId>(IHashHandler hashHandler) : ITrackerService<TId> where TId : new()
{
    public bool IsTracked(Entity<TId> entity, ITrackerService<TId>.TrackerAlgorithm strategy)
    {
        if (entity is null)
            return false;

        switch (strategy)
        {
            case ITrackerService<TId>.TrackerAlgorithm.Id:
                return entity.Id != null;
            case ITrackerService<TId>.TrackerAlgorithm.Hash:
                return hashHandler.IsHash(entity.Hash);
            case ITrackerService<TId>.TrackerAlgorithm.StringComparision:
                // TODO
                return false;
            case ITrackerService<TId>.TrackerAlgorithm.ContentSimilarity:
                return false;
            case ITrackerService<TId>.TrackerAlgorithm.CosineSimilarity:
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

        return IsTracked(entity, strategy: ITrackerService<TId>.TrackerAlgorithm.Id)
            || IsTracked(entity, strategy: ITrackerService<TId>.TrackerAlgorithm.Hash)
            || IsTracked(entity, strategy: ITrackerService<TId>.TrackerAlgorithm.StringComparision)
            || IsTracked(entity, strategy: ITrackerService<TId>.TrackerAlgorithm.ContentSimilarity)
            || IsTracked(entity, strategy: ITrackerService<TId>.TrackerAlgorithm.CosineSimilarity);
    }


}
