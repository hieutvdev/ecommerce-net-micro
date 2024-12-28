using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

public class KeyConfiguration : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).HasConversion(
            v => v.Value,
            dbId => KeyId.Of(dbId));

        builder.Property(k => k.Token).IsRequired();
   
    }
}