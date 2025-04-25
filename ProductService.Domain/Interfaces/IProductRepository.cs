using ClientsService.Domain.Interfaces;
using ClientsService.Domain.SoftDelete;
using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<IEnumerable<Product>> GetProductsByUserIdAsync(string id, CancellationToken cancellationToken = default);
}