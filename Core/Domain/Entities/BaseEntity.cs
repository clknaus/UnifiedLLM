namespace Core.Domain.Entities;

public abstract class BaseEntity : Entity<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }
}
