using Inventory.Domain.Aggregates;

namespace Inventory.Domain.Events.WarehouseProductEvents;

public record WarehouseProductCreateEvent() : IDomainEvent;
