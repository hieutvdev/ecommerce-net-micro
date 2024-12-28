
using Inventory.Domain.Models;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.EFConfigurations;

public class SuppliersConfiguration : IEntityTypeConfiguration<Suppliers>
{
    public void Configure(EntityTypeBuilder<Suppliers> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                supplierId => supplierId.Value,
                dbId => SupplierId.Of(dbId));

        builder.Property(s => s.SupplierName)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(sp => sp.ProductSuppliers)
            .WithOne()
            .HasForeignKey(sp => sp.SupplierId);

        // Configure SupplierAddress as a required complex property
        builder.OwnsOne(s => s.SupplierAddress, addressBuilder =>
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
        });  // Ensure the entire complex property is required

        // Configure ContactInfo as a required complex property
        builder.OwnsOne(s => s.ContactInfo, contactInfoBuilder =>
        {
            contactInfoBuilder.Property(a => a.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();

            contactInfoBuilder.Property(a => a.Email)
                .HasMaxLength(50)
                .IsRequired();
        });  // Ensure the entire complex property is required
    }
}
