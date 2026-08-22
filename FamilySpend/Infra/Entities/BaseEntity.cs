namespace FamilySpend.Infra.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastModified { get; set; } = DateTimeOffset.UtcNow;
}