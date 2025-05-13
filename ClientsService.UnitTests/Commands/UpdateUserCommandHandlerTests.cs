using ClientsService.Application.Commands;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    [Fact]
    public async Task Handle_ShouldUpdateUsername_WhenUserExists()
    {
        var user = new User { Email = "test@example.com", Username = "OldName" };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);
        await handler.Handle(new UpdateUserCommand { Email = user.Email, NewUserName = "NewName" }, CancellationToken.None);

        Assert.Equal("NewName", user.Username);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}
