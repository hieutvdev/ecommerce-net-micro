using Inventory.Domain.Models;

namespace Inventory.Infrastructure.Configurations;

public static class CacheKey
{
    public static string WarehouseKey { get; private set; } = $"{nameof(Warehouse)}___";
    public static string SupplierKey { get; private set; } = $"{nameof(Suppliers)}___";
    public static string WarehouseProduct { get; private set; } = $"{nameof(WarehouseProducts)}___";
    public static string ProductSupplierKey { get; private set; } = $"{nameof(ProductSuppliers)}___";
} 