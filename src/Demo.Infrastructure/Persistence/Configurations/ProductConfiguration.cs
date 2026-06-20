using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id).ValueGeneratedOnAdd();
        builder.Property(product => product.PublicId).IsRequired();
        builder.HasIndex(product => product.PublicId).IsUnique();
        builder.HasIndex(product => product.Name).IsUnique();

        builder.Property(product => product.Name).HasMaxLength(100).IsRequired();
        builder.Property(product => product.Description).HasMaxLength(500);
        builder.Property(product => product.Price).HasPrecision(18, 2);
        builder.Property(product => product.Status).IsRequired();
    }
}
