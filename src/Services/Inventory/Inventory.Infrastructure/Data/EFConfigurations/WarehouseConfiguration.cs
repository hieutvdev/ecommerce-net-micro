using Inventory.Domain.Enums;
using Inventory.Domain.Models;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.EFConfigurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasConversion(
            warehouseId => warehouseId.Value, dbId => WarehouseId.Of(dbId));
        builder.Property(w => w.ManagerId).HasConversion(
          warehouseId => warehouseId.Value, dbId => ManagerId.Of(dbId));

        builder.HasMany(w => w.WarehouseProducts)
            .WithOne()
            .HasForeignKey(wp => wp.WarehouseId);
        
        builder.Property(w => w.WarehouseName).HasMaxLength(100).IsRequired();

        builder.ComplexProperty(w => w.Location, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50);

            addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            addressBuilder.Property(a => a.Country)
                .HasMaxLength(50);

            addressBuilder.Property(a => a.State)
                .HasMaxLength(50);

            addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(5)
                .IsRequired();
        });


        builder.Property(w => w.WarehouseStatus)
            .HasDefaultValue(WarehouseStatus.Active)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (WarehouseStatus)Enum.Parse(typeof(WarehouseStatus), dbStatus));
    }
}