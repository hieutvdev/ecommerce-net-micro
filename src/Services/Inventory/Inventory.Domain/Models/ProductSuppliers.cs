using Inventory.Domain.Aggregates;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Models;

public class ProductSuppliers : Entity<ProductSupplierId>
{
    public ProductId ProductId { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public decimal SupplyPrice { get; private set; }

    internal ProductSuppliers(ProductId productId, SupplierId supplierId, decimal supplyPrice)
    {
        Id = ProductSupplierId.Of(Guid.NewGuid());
        ProductId = productId;
        SupplierId = supplierId;
        SupplyPrice = supplyPrice;
    }
}