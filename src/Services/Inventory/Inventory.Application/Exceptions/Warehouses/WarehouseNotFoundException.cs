using BuildingBlocks.Exceptions;

namespace Inventory.Application.Exceptions.Warehouses;

public class WarehouseNotFoundException : NotFoundException
{
    public WarehouseNotFoundException(Guid id) : base("Warehouse", id)
    {
        
    }
}