using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Models;
using ProductService.Persistence.Data;
using ProductService.Persistence.Repositories;
using Xunit;

namespace ProductsService.UnitTests.Repositories
{
    public class ProductRepositoryTests
    {
        private ProductsDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ProductsDbContext(options);
        }

        private Product CreateProduct(int userId, string name)
        {
            return new Product
            {
                UserId = userId,
                ProductName = name,
            };
        }

        [Fact]
        public async Task GetProductsByUserIdAsync_ReturnsCorrectProducts()
        {
            // Arrange
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var product1 = CreateProduct(1, "P1");
            var product2 = CreateProduct(1, "P2");
            var product3 = CreateProduct(2, "P3");

            context.Products.AddRange(product1, product2, product3);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetProductsByUserIdAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(1, p.UserId));
        }

        [Fact]
        public async Task GetProductByNameAsync_ReturnsCorrectProduct()
        {
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var product = CreateProduct(5, "UniqueProduct");
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await repo.GetProductByNameAsync("UniqueProduct", 5);

            Assert.NotNull(result);
            Assert.Equal("UniqueProduct", result!.ProductName);
            Assert.Equal(5, result.UserId);
        }

        [Fact]
        public async Task GetProductByNameAsync_ReturnsNullIfNotFound()
        {
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var result = await repo.GetProductByNameAsync("DoesNotExist", 1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteRange_DeletesProducts()
        {
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var product1 = CreateProduct(1, "P1");
            var product2 = CreateProduct(1, "P2");
            context.Products.AddRange(product1, product2);
            await context.SaveChangesAsync();

            var allProducts = context.Products.ToList();
            await repo.DeleteRange(allProducts);

            Assert.Empty(context.Products);
        }

        [Fact]
        public async Task UpdateRange_UpdatesProducts()
        {
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var product = CreateProduct(1, "OldName");
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Change
            product.ProductName = "NewName";
            await repo.UpdateRange(new List<Product> { product });

            var updated = await context.Products.FirstOrDefaultAsync();
            Assert.Equal("NewName", updated!.ProductName);
        }

        [Fact]
        public async Task GetUserProductsWithoutFiltersAsync_IgnoresFilters()
        {
            var context = CreateDbContext();
            var repo = new ProductRepository(context);

            var product = CreateProduct(99, "HiddenProduct");
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await repo.GetUserProductsWithoutFiltersAsync(99);

            Assert.Single(result);
            Assert.Equal("HiddenProduct", result.First().ProductName);
        }
    }
}
