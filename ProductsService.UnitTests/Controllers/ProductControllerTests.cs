using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Application.Commands;
using ProductService.Application.DTOs;
using ProductService.Application.Queries;
using ProductsService.API.Controllers;
using Xunit;

namespace ProductsService.UnitTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductController(_mediatorMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("sub", "42")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsOkWithProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductDTO> { new ProductDTO {ProductName = "Test", Description = "Test", IsAvailable = true, CountInStock = 0, Price = 0 } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedProducts, okResult.Value);
        }

        [Fact]
        public async Task GetMyProducts_WithValidUser_ReturnsOk()
        {
            var expected = new List<ProductDTO> { new ProductDTO { ProductName = "MyProduct", Description = "Test", IsAvailable = true, CountInStock = 0, Price = 0} };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.GetMyProducts(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task GetMyProducts_WithNoUser_ReturnsUnauthorized()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // No ClaimsPrincipal
            };

            var result = await _controller.GetMyProducts(CancellationToken.None);

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_ValidUser_ReturnsNoContent()
        {
            var product = new ProductDTO { ProductName = "NewProduct",  Description = "Test", IsAvailable = true, CountInStock = 0, Price = 0 };

            var result = await _controller.CreateProduct(product, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AddProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_ValidUser_ReturnsNoContent()
        {
            var product = new ProductDTO { ProductName = "UpdatedProduct",  Description = "Test", IsAvailable = true, CountInStock = 0, Price = 0 };

            var result = await _controller.UpdateProduct(product, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_ValidUser_ReturnsNoContent()
        {
            var result = await _controller.DeleteProduct("ProductName", CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAllUserProducts_ValidUser_ReturnsNoContent()
        {
            var result = await _controller.DeleteAllUserProducts(CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserProductsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReturnUserProducts_ValidUser_ReturnsNoContent()
        {
            var result = await _controller.ReturnUserProducts(CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ReturnUserProductsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
