using Inventory.Domain.Exceptions;

namespace Inventory.Domain.ValueObjects;

public class ProductSupplierId
{
    public Guid Value { get; }

    private ProductSupplierId(Guid value) => Value = value;

    public static ProductSupplierId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
        {
            throw new DomainException("ProductSupplierId cannot be empty!");
        }

        return new ProductSupplierId(value);
    }
}