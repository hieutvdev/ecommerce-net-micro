namespace Inventory.Domain.Aggregates;

public class Aggregate<T> : Entity<T>, IAggregate<T>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeueEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeueEvents;
    }


}