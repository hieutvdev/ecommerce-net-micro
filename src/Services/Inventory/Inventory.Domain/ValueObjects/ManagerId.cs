using Inventory.Domain.Exceptions;

namespace Inventory.Domain.ValueObjects;

public class ManagerId
{
    public Guid Value { get; }

    private ManagerId(Guid value) => Value = value;

    public static ManagerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("ManagerId cannot be empty!");
        }

        return new ManagerId(value);
    }
}