using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using Moq;

namespace ClientsService.UnitTests.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();

    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenEmailIsUnique()
    {
        var dto = new RegisterUserDTO { Email = "test@example.com", Password = "123", Role = "User", UserName = "Name" };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(dto.Email, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        _passwordServiceMock.Setup(p => p.HashPassword(dto.Password)).Returns("hashed");
        _tokenServiceMock.Setup(t => t.GenerateJwtToken(It.IsAny<User>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new TokenDTO(null, null));

        var handler = new RegisterUserCommandHandler(_passwordServiceMock.Object, _userRepositoryMock.Object, _tokenServiceMock.Object);
        var result = await handler.Handle(new RegisterUserCommand { RegisterUser = dto }, CancellationToken.None);

        Assert.NotNull(result);
        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == dto.Email), It.IsAny<CancellationToken>()), Times.Once);
    }
}
