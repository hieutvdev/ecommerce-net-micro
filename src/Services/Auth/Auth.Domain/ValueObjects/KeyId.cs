namespace Auth.Domain.ValueObjects;

public record KeyId
{
    public Guid Value { get; }
    private KeyId(Guid value) => Value = value;


    public static KeyId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("KeyId is required!");
        }

        return new KeyId(value);
    }
}