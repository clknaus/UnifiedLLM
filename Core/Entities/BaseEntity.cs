namespace Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }
}
