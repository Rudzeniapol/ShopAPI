using Moq;
using ProductService.Application.Commands;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Commands;

public class ReturnUserProductsCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task Handle_ShouldUpdateProducts()
    {
        var handler = new ReturnUserProductsCommandHandler(_productRepo.Object);
        var products = new List<Product>
        {
            new Product { IsDeleted = true, DeletedAt = DateTime.UtcNow }
        };

        _productRepo.Setup(r => r.GetUserProductsWithoutFiltersAsync(1, default)).ReturnsAsync(products);

        await handler.Handle(new ReturnUserProductsCommand { UserId = 1 }, default);

        Assert.All(products, p => Assert.False(p.IsDeleted));
        _productRepo.Verify(r => r.UpdateRange(products, default), Times.Once);
    }
}
