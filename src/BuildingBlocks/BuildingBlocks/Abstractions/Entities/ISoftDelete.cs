namespace BuildingBlocks.Abstractions.Entities;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}