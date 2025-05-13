using System.Threading;
using System.Threading.Tasks;
using ClientsService.API.Controllers;
using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClientsService.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsTokenDTO()
        {
            // Arrange
            var user = new LoginUserDTO()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };
            var command = new LoginUserCommand
            {
                LoginUser = user
            };

            var expectedToken = new TokenDTO("access-token", "refresh-token");

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualToken = Assert.IsType<TokenDTO>(okResult.Value);
            Assert.Equal(expectedToken.AccessToken, actualToken.AccessToken);
        }

        [Fact]
        public async Task Register_ReturnsTokenDTO()
        {
            // Arrange
            var user = new RegisterUserDTO()
            {
                Email = "newuser@example.com",
                Password = "Password123!",
                Role = "admin",
                UserName = "newUser"
            };
            var command = new RegisterUserCommand
            {
                RegisterUser = user
            };

            var expectedToken = new TokenDTO("access-token","refresh-token");
            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Register(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualToken = Assert.IsType<TokenDTO>(okResult.Value);
            Assert.Equal(expectedToken.RefreshToken, actualToken.RefreshToken);
        }
    }
}
