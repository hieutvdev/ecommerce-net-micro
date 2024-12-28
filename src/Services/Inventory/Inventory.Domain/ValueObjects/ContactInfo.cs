namespace Inventory.Domain.ValueObjects;

public record ContactInfo
{
    public string PhoneNumber { get; } = default!;
    public string Email { get; } = default!;


    protected ContactInfo()
    {
        
    }


    private ContactInfo(string phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }


    public static ContactInfo Of(string phoneNumber, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new ContactInfo(phoneNumber, email);
    }
}