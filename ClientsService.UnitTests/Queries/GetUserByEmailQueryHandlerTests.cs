using AutoMapper;
using ClientsService.Application.DTOs;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Queries;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Queries;

public class GetUserByEmailQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnUserDTO_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User { Email = email, Username = "TestUser" };
        var userDto = new UserDTO { Email = email, UserName = "TestUser" };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDto);

        var handler = new GetUserByEmailQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetUserByEmailQuery { Email = email }, CancellationToken.None);

        // Assert
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal(userDto.UserName, result.UserName);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new GetUserByEmailQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetUserByEmailQuery { Email = "notfound@example.com" }, CancellationToken.None));
    }
}
