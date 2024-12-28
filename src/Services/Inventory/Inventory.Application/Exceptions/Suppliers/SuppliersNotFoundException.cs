using BuildingBlocks.Exceptions;

namespace Inventory.Application.Exceptions.Suppliers;

public class SuppliersNotFoundException : NotFoundException
{
    public SuppliersNotFoundException(Guid id) : base("Supplier", id)
    {
        
    }
}