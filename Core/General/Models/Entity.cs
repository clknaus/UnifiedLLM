namespace Core.General.Models;
public abstract class Entity<TId> where TId : new()
{
    public virtual TId Id { get; private set; } = new();

    protected Entity()
    {
        Id = (typeof(TId) == typeof(Guid)) ? (TId)(object)Guid.NewGuid() : default!;
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);
}