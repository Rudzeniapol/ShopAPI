using AutoMapper;
using Moq;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductsService.UnitTests.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IMapper> _mapper = new();

        [Fact]
        public async Task Handle_ShouldReturnMappedProducts()
        {
            var products = new List<Product> { new Product(), new Product() };
            var dtoList = new List<ProductDTO> { new(), new() };

            _productRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);
            _mapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(products)).Returns(dtoList);

            var handler = new GetAllProductsQueryHandler(_productRepo.Object, _mapper.Object);
            var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            Assert.Equal(dtoList, result);
        }
    }
}