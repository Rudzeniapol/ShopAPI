using ClientsService.Application.Clients.Interfaces;
using ClientsService.Application.Commands;
using ClientsService.Application.Exceptions;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class ActivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IProductServiceClient> _productServiceClientMock = new();

    [Fact]
    public async Task Handle_ShouldActivateUser_WhenUserExists()
    {
        var user = new User { Email = "test@example.com", IsActivated = false };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new ActivateUserCommandHandler(_userRepositoryMock.Object, _productServiceClientMock.Object);
        await handler.Handle(new ActivateUserCommand { Email = user.Email }, CancellationToken.None);

        Assert.True(user.IsActivated);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new ActivateUserCommandHandler(_userRepositoryMock.Object, _productServiceClientMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new ActivateUserCommand { Email = "missing@example.com" }, CancellationToken.None));
    }
}
