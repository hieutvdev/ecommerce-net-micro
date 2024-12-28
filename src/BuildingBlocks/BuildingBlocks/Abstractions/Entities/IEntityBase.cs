namespace BuildingBlocks.Abstractions.Entities;

public interface IEntityBase<T>
{
    T Id { get; set; }
}