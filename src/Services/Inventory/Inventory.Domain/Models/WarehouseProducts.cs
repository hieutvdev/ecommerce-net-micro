using Inventory.Domain.Aggregates;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Models;

public class WarehouseProducts : Aggregate<WarehouseProductsId>
{
    public WarehouseId WarehouseId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;

    internal WarehouseProducts(WarehouseId warehouseId, ProductId productId, int quantity)
    {
        Id = WarehouseProductsId.Of(Guid.NewGuid());
        WarehouseId = warehouseId;
        ProductId = productId;
        Quantity = quantity;
    }
}