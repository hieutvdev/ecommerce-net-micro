using Inventory.Application.Dtos.Address;
using Inventory.Domain.Enums;

namespace Inventory.Application.Dtos.Warehouses;

public record WarehouseDto(
    Guid WarehouseId,
    string WarehouseName,
    Guid ManagerId,
    WarehouseStatus WarehouseStatus,
    AddressDto Address
    );
