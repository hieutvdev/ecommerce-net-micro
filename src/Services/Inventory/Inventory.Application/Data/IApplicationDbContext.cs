
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Data;

public interface  IApplicationDbContext
{
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WarehouseProducts> WarehouseProducts { get; }
    DbSet<Suppliers> Suppliers { get; }
    DbSet<ProductSuppliers> ProductSuppliers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}