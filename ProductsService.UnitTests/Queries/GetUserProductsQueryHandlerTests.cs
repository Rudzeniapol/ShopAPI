using AutoMapper;
using Moq;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Queries;

public class GetUserProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task Handle_ShouldReturnUserProducts()
    {
        var products = new List<Product> { new Product(), new Product() };
        var productDtos = new List<ProductDTO> { new(), new() };

        _productRepo.Setup(r => r.GetProductsByUserIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(products)).Returns(productDtos);

        var handler = new GetUserProductsQueryHandler(_productRepo.Object, _mapper.Object);
        var result = await handler.Handle(new GetUserProductsQuery { UserId = 1 }, CancellationToken.None);

        Assert.Equal(productDtos, result);
    }
}
