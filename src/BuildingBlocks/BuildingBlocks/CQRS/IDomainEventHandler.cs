using MediatR;

namespace BuildingBlocks.CQRS;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
where TDomainEvent  : IDomainEvent
{
    
}