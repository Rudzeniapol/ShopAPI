using ClientsService.Application.Commands;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        var user = new User { Email = "delete@example.com" };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
        await handler.Handle(new DeleteUserCommand { Email = user.Email }, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.DeleteAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}
