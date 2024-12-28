namespace Inventory.Domain.Aggregates;

public class Entity<TId> : IEntity<TId>
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool? Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public TId Id { get; set; }
}