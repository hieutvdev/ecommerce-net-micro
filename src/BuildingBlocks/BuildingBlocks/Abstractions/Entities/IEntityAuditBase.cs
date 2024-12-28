namespace BuildingBlocks.Abstractions.Entities;

public interface IEntityAuditBase<T> : IEntityBase<T>, IAuditable
{
    
}