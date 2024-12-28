namespace BuildingBlocks.Abstractions.Entities;

public interface IDateTracking
{
    DateTimeOffset CreatedDate { get; set; }
    DateTimeOffset? UpdatedDate { get; set; }
}