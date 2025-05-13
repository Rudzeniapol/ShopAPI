using ClientsService.Application.Clients.Interfaces;
using ClientsService.Application.Commands;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class DeactivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IProductServiceClient> _productServiceClientMock = new();

    [Fact]
    public async Task Handle_ShouldDeactivateUser_WhenUserExists()
    {
        var user = new User { Email = "test@example.com", IsActivated = true };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new DeactivateUserCommandHandler(_userRepositoryMock.Object, _productServiceClientMock.Object);
        await handler.Handle(new DeactivateUserCommand { Email = user.Email }, CancellationToken.None);

        Assert.False(user.IsActivated);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}
