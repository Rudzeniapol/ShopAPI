using AutoMapper;
using Moq;
using ProductService.Application.Commands;
using ProductService.Application.DTOs;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenProductNotFound()
    {
        var handler = new UpdateProductCommandHandler(_productRepo.Object, _mapper.Object);
        var command = new UpdateProductCommand
        {
            Product = new ProductDTO { ProductName = "Test" },
            UserId = 1
        };

        _productRepo.Setup(r => r.GetProductByNameAsync("Test", 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductExists()
    {
        var existing = new Product { ProductName = "Test" };
        var command = new UpdateProductCommand
        {
            Product = new ProductDTO
            {
                ProductName = "Test",
                Description = "New desc",
                Price = 10,
                IsAvailable = true,
                CountInStock = 5
            },
            UserId = 1
        };

        _productRepo.Setup(r => r.GetProductByNameAsync("Test", 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var handler = new UpdateProductCommandHandler(_productRepo.Object, _mapper.Object);

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("New desc", existing.Description);
        Assert.Equal(10, existing.Price);
        Assert.True(existing.IsAvailable);
        Assert.Equal(5, existing.CountInStock);

        _productRepo.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }
}
