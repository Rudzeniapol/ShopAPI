using AutoMapper;
using ClientsService.Application.DTOs;
using ClientsService.Application.Queries;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Queries;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnListOfUserDTOs()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Email = "user1@example.com", Username = "User1" },
            new() { Email = "user2@example.com", Username = "User2" }
        };

        var usersDto = new List<UserDTO>
        {
            new() { Email = "user1@example.com", UserName = "User1" },
            new() { Email = "user2@example.com", UserName = "User2" }
        };

        _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(users)).Returns(usersDto);

        var handler = new GetUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Collection(result,
            item => Assert.Equal("user1@example.com", item.Email),
            item => Assert.Equal("user2@example.com", item.Email));
    }
}
