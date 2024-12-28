namespace Inventory.Domain.Aggregates;

public interface IAggregate<T> : IEntity<T>, IAggregate
{
    
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}