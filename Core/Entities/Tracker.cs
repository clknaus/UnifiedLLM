using System.Linq.Expressions;

namespace Core.Entities;
internal class Tracker(inject context) : Entity<Guid>
{
    // Id
    Hash Hash { get; set; } // Dot Net 10, inherent entity<guid>
    Probability Probability { get; set; }
    // Content similarity
    // Cosine Probability

    // IsTracked
    bool IsTracked(TrackerAlgorithm strategy)
    {
        throw new NotImplementedException();
        return false;

        switch (strategy)
        {
            case TrackerAlgorithm.Id:
                // code block
                break;
            case TrackerAlgorithm.Hash:
                // code block
                break;
            case TrackerAlgorithm.StringComparision:
                // code block
                break;
            case TrackerAlgorithm.ContentSimilarity:
                // code block
                break;
            case TrackerAlgorithm.CosineSimilarity:
                // code block
                break;
            default:
                // TrackerAlgorithm.Default
                // chain / builer pattern
                return IsEntity();
                break;
        }

        bool IsEntity() {
        {
                var entity = context.GetById(base.Id);
                if (entity == null)
                {
                    return false;
                }

                return
                    Id == entity.Id ||
                    Hash == entity.Hash;
                    
                    //TODO
                    //IsStringComparision(approve: Number value, entity.StringComparisionProbability) ||
                    //IsContentSimilarity(entity.ContentSimilarity) ||
                    //IsCosineSimilarity(entity.ConsinePropbability);
            }

        }
}

public enum TrackerAlgorithm
{
    Default = 0,
    Id = 1,
    Hash = 2,
    StringComparision = 3,
    ContentSimilarity = 4,
    CosineSimilarity = 5
}