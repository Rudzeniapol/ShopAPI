using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using ClientsService.API;
using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Domain.Models;
using ClientsService.Persistence.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using BCrypt.Net;

namespace ClientsService.IntegrationTests;

public class UserControllerTests : TestBase
{
    private User _testUser;
    private string _authToken;
    private const string TestPassword = "password123";

    public UserControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        // Создаем тестового пользователя
        _testUser = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(TestPassword), // Хешируем пароль
            Role = "admin",
            IsActivated = true
        };
        DbContext.Users.Add(_testUser);
        DbContext.SaveChanges();

        // Создаем тестовый JWT токен
        _authToken = GenerateTestToken();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authToken}");
    }

    private string GenerateTestToken()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "1"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim(ClaimTypes.Email, _testUser.Email)
        };

        // Используем более длинный ключ (минимум 32 байта для HS256)
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("your-super-secret-key-with-minimum-32-bytes-length-for-hs256"));

        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
            key, 
            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "test-issuer",
            audience: "test-audience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsCreated()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            RegisterUser = new RegisterUserDTO
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                Password = TestPassword,
                Role = "user"
            }
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/register", command);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<TokenDTO>();
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            LoginUser = new LoginUserDTO
            {
                Email = "test@example.com",
                Password = TestPassword
            }
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<TokenDTO>();
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task GetUser_ValidEmail_ReturnsUser()
    {
        // Act
        var response = await Client.GetAsync($"/api/clients/user/{_testUser.Email}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        Assert.NotNull(user);
        Assert.Equal(_testUser.Email, user.Email);
        Assert.Equal(_testUser.Username, user.UserName);
        Assert.Equal(_testUser.Role, user.Role);
    }

    [Fact]
    public async Task UpdateUser_ValidData_ReturnsNoContent()
    {
        // Arrange
        var newUserName = "updateduser";

        // Act
        var response = await Client.PutAsync($"/api/clients/user/{_testUser.Email}?newUserName={newUserName}", null);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ValidEmail_ReturnsNoContent()
    {
        // Act
        var response = await Client.DeleteAsync($"/api/clients/user/{_testUser.Email}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}