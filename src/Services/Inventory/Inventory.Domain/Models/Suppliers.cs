using Inventory.Domain.Aggregates;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Models;
#nullable disable
public class Suppliers : Entity<SupplierId>
{
    private readonly List<ProductSuppliers> _productSuppliers = new();
    public IReadOnlyList<ProductSuppliers> ProductSuppliers => _productSuppliers.AsReadOnly();

    public string SupplierName { get; private set; } = default!;
    public Address SupplierAddress { get; private set; } = default!;
    public ContactInfo ContactInfo { get; private set; } = default!;


    public static Suppliers Create(SupplierId id, string supplierName, Address address, ContactInfo contactInfo)
    {
        var supplier = new Suppliers
        {
            Id = id,
            SupplierName = supplierName,
            SupplierAddress = address,
            ContactInfo = contactInfo

        };
        return supplier;
    }


    public void Add(ProductId productId, decimal supplyPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(supplyPrice);

        var productSupplier = new ProductSuppliers(productId, this.Id, supplyPrice);
        
        _productSuppliers.Add(productSupplier);
    }

    public void Remove(ProductId productId)
    {
        var productSupplier = _productSuppliers.FirstOrDefault(x => x.ProductId == productId);
        if (productSupplier is not null)
        {
            _productSuppliers.Remove(productSupplier);
        }
    }
    
}