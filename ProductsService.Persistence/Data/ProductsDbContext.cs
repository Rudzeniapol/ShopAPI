using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Models;
using ProductService.Persistence.Configurations;
using ProductService.Persistence.Interceptors;

namespace ProductService.Persistence.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasQueryFilter(x => x.IsDeleted == false);
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}