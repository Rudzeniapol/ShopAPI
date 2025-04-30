using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;
using ProductService.Persistence.Data;

namespace ProductService.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ProductsDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.UserId == id).ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByNameAsync(string name, int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p=> p.UserId == userId && p.ProductName.Equals(name), cancellationToken);
    }

    public async Task DeleteRange(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(products);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetUserProductsWithoutFiltersAsync(int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.IgnoreQueryFilters().Where(p => p.UserId == id).ToListAsync(cancellationToken);
    }
    
    public async Task UpdateRange(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(products);
        await _context.SaveChangesAsync(cancellationToken);
    }
}