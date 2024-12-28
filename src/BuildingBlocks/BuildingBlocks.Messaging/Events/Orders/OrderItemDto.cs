

namespace BuildingBlocks.Messaging.Events.Orders;

public record OrderItemDto(Guid OrderId, Guid ProductId, int Quantity, decimal Price);