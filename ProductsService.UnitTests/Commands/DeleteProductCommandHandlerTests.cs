using Moq;
using ProductService.Application.Commands;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenProductNotFound()
    {
        var handler = new DeleteProductCommandHandler(_productRepo.Object);
        var command = new DeleteProductCommand { ProductName = "Test", UserId = 1 };

        _productRepo.Setup(r => r.GetProductByNameAsync(command.ProductName, command.UserId, default))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenProductFound()
    {
        var handler = new DeleteProductCommandHandler(_productRepo.Object);
        var command = new DeleteProductCommand { ProductName = "Test", UserId = 1 };
        var product = new Product();

        _productRepo.Setup(r => r.GetProductByNameAsync(command.ProductName, command.UserId, default))
            .ReturnsAsync(product);

        await handler.Handle(command, default);

        _productRepo.Verify(r => r.DeleteAsync(product, default), Times.Once);
    }
}
