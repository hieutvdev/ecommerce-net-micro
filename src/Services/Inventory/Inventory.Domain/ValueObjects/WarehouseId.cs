using Inventory.Domain.Exceptions;

namespace Inventory.Domain.ValueObjects;

public class WarehouseId
{
    public Guid Value { get; }

    private WarehouseId(Guid value) => Value = value;


    public static WarehouseId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("WarehouseId cannot be empty");
        }

        return new WarehouseId(value);
    }
}