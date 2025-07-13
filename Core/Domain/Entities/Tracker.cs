namespace Core.Domain.Entities;
public class Tracker() : Entity<Guid>
{
    // Id
    public string Hash { get; set; }
    public Probability Probability { get; set; }
    // Content similarity
    // Cosine Probability

    // IsTracked

}