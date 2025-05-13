using Moq;
using ProductService.Application.Commands;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Commands;

public class DeleteUserProductsCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenNoProducts()
    {
        var handler = new DeleteUserProductsCommandHandler(_productRepo.Object);

        _productRepo.Setup(r => r.GetProductsByUserIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(new List<Product>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeleteUserProductsCommand { UserId = 1 }, default));
    }

    [Fact]
    public async Task Handle_ShouldDeleteRange_WhenProductsExist()
    {
        var handler = new DeleteUserProductsCommandHandler(_productRepo.Object);
        var products = new List<Product> { new(), new() };

        _productRepo.Setup(r => r.GetProductsByUserIdAsync(1, default)).ReturnsAsync(products);

        await handler.Handle(new DeleteUserProductsCommand { UserId = 1 }, default);

        _productRepo.Verify(r => r.DeleteRange(products, default), Times.Once);
    }
}
