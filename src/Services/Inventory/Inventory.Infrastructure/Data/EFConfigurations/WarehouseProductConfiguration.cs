
using Inventory.Domain.Models;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.EFConfigurations;

public class WarehouseProductConfiguration : IEntityTypeConfiguration<WarehouseProducts>
{
    public void Configure(EntityTypeBuilder<WarehouseProducts> builder)
    {
        builder.HasKey(wp => wp.Id);
        builder.Property(wp => wp.Id).HasConversion(warehouseProductId => warehouseProductId.Value,
            dbId => WarehouseProductsId.Of(dbId));
        builder.Property(wp => wp.ProductId).HasConversion(productId => productId.Value,
          dbId => ProductId.Of(dbId));
        builder.Property(wp => wp.Quantity).IsRequired();
    }
}