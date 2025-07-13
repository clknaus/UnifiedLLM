using Core.Domain.Entities;
using Core.General.Interfaces;
using static Application.Services.ITrackerService;

namespace Application.Services;
public class TrackerService(IHashHandler hashHandler) : ITrackerService
{
    public bool IsTracked(Tracker tracker, TrackerAlgorithm strategy)
    {
        if (tracker is null)
            return false;

        switch (strategy)
        {
            case TrackerAlgorithm.Id:
                return tracker.Id.Variant > 0;
            case TrackerAlgorithm.Hash:
                return hashHandler.IsHash(tracker.Hash);
            case TrackerAlgorithm.StringComparision:
                // TODO
                return false;
            case TrackerAlgorithm.ContentSimilarity:
                return false;
            case TrackerAlgorithm.CosineSimilarity:
                return false;
            default:
                // TrackerAlgorithm.Default
                // chain / builer pattern
                return IsTracked(tracker);
        }
    }

    public bool IsTracked(Tracker tracker)
    {
        if (tracker == null)
            return false;

        return IsTracked(tracker, strategy: TrackerAlgorithm.Id)
            || IsTracked(tracker, strategy: TrackerAlgorithm.Hash)
            || IsTracked(tracker, strategy: TrackerAlgorithm.StringComparision)
            || IsTracked(tracker, strategy: TrackerAlgorithm.ContentSimilarity)
            || IsTracked(tracker, strategy: TrackerAlgorithm.CosineSimilarity);
    }


}
