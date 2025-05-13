using AutoMapper;
using Moq;
using ProductService.Application.Commands;
using ProductService.Application.DTOs;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Commands;

public class AddProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenProductExists()
    {
        var handler = new AddProductCommandHandler(_productRepositoryMock.Object, _mapperMock.Object);
        var command = new AddProductCommand { Product = new(), UserId = 1 };

        _productRepositoryMock
            .Setup(r => r.GetProductByNameAsync(It.IsAny<string>(), It.IsAny<int>(), default))
            .ReturnsAsync(new Product());

        await Assert.ThrowsAsync<EntityExistsException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldAddProduct_WhenProductNotExists()
    {
        var handler = new AddProductCommandHandler(_productRepositoryMock.Object, _mapperMock.Object);
        var command = new AddProductCommand
        {
            Product = new ProductDTO { ProductName = "Test" },
            UserId = 1
        };

        _productRepositoryMock
            .Setup(r => r.GetProductByNameAsync(command.Product.ProductName, command.UserId, default))
            .ReturnsAsync((Product?)null);

        _mapperMock.Setup(m => m.Map<Product>(command.Product))
            .Returns(new Product { ProductName = "Test" });

        await handler.Handle(command, default);

        _productRepositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.ProductName == "Test" && p.IsAvailable && p.UserId == 1), default), Times.Once);
    }
}
