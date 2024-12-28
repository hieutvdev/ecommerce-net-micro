using Inventory.Domain.Aggregates;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Models;

public class Warehouse : Entity<WarehouseId>
{

    private readonly List<WarehouseProducts> _warehouseProducts = new();
    public IReadOnlyList<WarehouseProducts> WarehouseProducts => _warehouseProducts.AsReadOnly();
    
    public string WarehouseName { get; private set; } = default!;
    public ManagerId ManagerId { get; private set; } = default!;
    public Address Location { get; private set; } = default!;
    public WarehouseStatus WarehouseStatus { get; private set; } = WarehouseStatus.Active;


    public static Warehouse Create(WarehouseId id, string name, Address location, ManagerId managerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);


        var warehouse = new Warehouse
        {
            Id = id,
            WarehouseName = name,
            ManagerId = managerId,
            Location = location
        };

        return warehouse;
    }

    public void Add(WarehouseId warehouseId, ProductId productId, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var warehouseProduct = new WarehouseProducts(warehouseId, productId, quantity);
        
        _warehouseProducts.Add(warehouseProduct);
    }

    public void Remove(ProductId productId)
    {
        var wareHouseProduct = _warehouseProducts.FirstOrDefault(x => x.ProductId == productId);
        if (wareHouseProduct is not null)
        {
            _warehouseProducts.Remove(wareHouseProduct);
        }
    }
}