using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<IEnumerable<Product>> GetProductsByUserIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<Product?> GetProductByNameAsync(string name, int userId, CancellationToken cancellationToken = default);
    public Task DeleteRange(IEnumerable<Product> products, CancellationToken cancellationToken = default);
    public Task UpdateRange(IEnumerable<Product> products, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Product>> GetUserProductsWithoutFiltersAsync(int id,
        CancellationToken cancellationToken = default);
}