using MediatR;

namespace BuildingBlocks.CQRS;

public interface IDomainEvent : INotification
{
    public Guid IdEvent { get; init; }
}