
using Inventory.Domain.Models;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.EFConfigurations;

public class ProductSuppliersConfiguration : IEntityTypeConfiguration<ProductSuppliers>
{
    public void Configure(EntityTypeBuilder<ProductSuppliers> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasConversion(productSupplierId => productSupplierId.Value,
            dbId => ProductSupplierId.Of(dbId));
        builder.Property(sp => sp.ProductId).IsRequired();
        builder.Property(sp => sp.ProductId).HasConversion(productId => productId.Value,
            dbId => ProductId.Of(dbId));
        builder.Property(sp => sp.SupplyPrice).IsRequired();
    }
}