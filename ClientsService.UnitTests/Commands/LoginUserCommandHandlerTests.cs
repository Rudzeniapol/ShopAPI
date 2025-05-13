using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsValid()
    {
        var loginDto = new LoginUserDTO { Email = "test@example.com", Password = "pass" };
        var user = new User { Email = loginDto.Email, PasswordHash = "hash" };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(loginDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordServiceMock.Setup(p => p.VerifyPassword(user.PasswordHash, loginDto.Password)).Returns(true);
        _tokenServiceMock.Setup(t => t.GenerateJwtToken(user, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TokenDTO(null, null));

        var handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _tokenServiceMock.Object, _passwordServiceMock.Object);
        var result = await handler.Handle(new LoginUserCommand { LoginUser = loginDto }, CancellationToken.None);

        Assert.NotNull(result);
    }
}
