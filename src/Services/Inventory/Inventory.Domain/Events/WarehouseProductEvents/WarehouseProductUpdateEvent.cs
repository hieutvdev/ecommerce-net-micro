using Inventory.Domain.Aggregates;

namespace Inventory.Domain.Events.WarehouseProductEvents;

public record WarehouseProductUpdateEvent() : IDomainEvent;
