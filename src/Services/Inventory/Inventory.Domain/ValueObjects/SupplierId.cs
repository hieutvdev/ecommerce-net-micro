using Inventory.Domain.Exceptions;

namespace Inventory.Domain.ValueObjects;

public class SupplierId
{
    public Guid Value { get; }

    private SupplierId(Guid value) => Value = value;


    public static SupplierId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("SupplierId cannot be empty!");
        }

        return new SupplierId(value);
    }
}