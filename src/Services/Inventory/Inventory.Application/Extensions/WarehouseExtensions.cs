using Inventory.Application.Dtos.Address;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Extensions;

public static class WarehouseExtensions
{
    public static AddressDto AddressToDto(Address address)
        => new AddressDto(address.FirstName, address.LastName, address.EmailAddress!, address.AddressLine,
            address.Country, address.State, address.ZipCode);
}