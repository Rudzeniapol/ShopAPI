using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Models;

namespace ProductService.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.CountInStock)
            .IsRequired();
        builder.Property(e => e.IsAvailable)
            .IsRequired();
        builder.Property(e => e.CreatedOn)
            .IsRequired();
        builder.Property(p => p.UserId)
            .IsRequired();
        builder.HasIndex(p => p.UserId);
    }
}