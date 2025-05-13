using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClientsService.API.Controllers;
using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClientsService.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            var expectedUsers = new List<UserDTO>
            {
                new UserDTO { Email = "test@example.com", UserName = "TestUser", IsActive = true, Role = "admin"}
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetUsers(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualUsers = Assert.IsAssignableFrom<List<UserDTO>>(okResult.Value);
            Assert.Single(actualUsers);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnsUser()
        {
            // Arrange
            string email = "test@example.com";
            var expectedUser = new UserDTO { Email = email, UserName = "TestUser" };
            _mediatorMock.Setup(m => m.Send(It.Is<GetUserByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.GetUserByEmail(email, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(email, actualUser.Email);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent()
        {
            // Arrange
            string email = "test@example.com";
            string newUserName = "NewName";
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(email, newUserName, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            // Arrange
            string email = "delete@example.com";
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(email, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeactivateUser_ReturnsNoContent()
        {
            // Arrange
            string email = "deactivate@example.com";
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeactivateUserCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeactivateUser(email, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ActivateUser_ReturnsNoContent()
        {
            // Arrange
            string email = "activate@example.com";
            _mediatorMock.Setup(m => m.Send(It.IsAny<ActivateUserCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ActivateUser(email, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
