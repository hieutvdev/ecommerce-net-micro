using Inventory.Domain.Exceptions;

namespace Inventory.Domain.ValueObjects;

public class WarehouseProductsId
{
    public Guid Value { get; }
    private WarehouseProductsId(Guid value) => Value = value;

    public static WarehouseProductsId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("WarehouseProductId Cannot be empty!");
            
        }

        return new WarehouseProductsId(value);
    }
}