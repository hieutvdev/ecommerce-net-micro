using Inventory.Application.Dtos.Address;
using Inventory.Domain.Enums;


namespace Inventory.Application.Dtos.Warehouses.Requests;
public record CreateWarehouseRequest(
    string WarehouseName,
    Guid ManagerId,
    WarehouseStatus WarehouseStatus,
    AddressDto Address);

