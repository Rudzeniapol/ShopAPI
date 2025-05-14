using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.DTOs;
using ProductService.Domain.Models;
using ProductService.Persistence.Data;
using ProductsService.API;
using Xunit;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Microsoft.IdentityModel.Tokens;

namespace ProductsService.IntegrationTests;

public class ProductControllerTests : TestBase
{
    private Product _testProduct;
    private string _authToken;

    public ProductControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        // Создаем тестовый продукт
        _testProduct = new Product
        {
            ProductName = "Test Product",
            Description = "Test Description",
            Price = 100,
            CountInStock = 10,
            CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow),
            IsAvailable = true,
            UserId = 1 // Добавляем UserId, соответствующий ID в JWT токене
        };
        DbContext.Products.Add(_testProduct);
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
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("super_mega_secret_key_1234567890"));

        var creds = new SigningCredentials(
            key, 
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "InnoShop",
            audience: "Clients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task GetProducts_ReturnsListOfProducts()
    {
        // Act
        var response = await Client.GetAsync("/api/products/product");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task GetMyProducts_ReturnsUserProducts()
    {
        // Act
        var response = await Client.GetAsync("/api/products/product/myProducts");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
        Assert.NotNull(products);
    }

    [Fact]
    public async Task CreateProduct_ValidData_ReturnsCreated()
    {
        // Arrange
        var product = new ProductDTO
        {
            ProductName = "New Product",
            Description = "New Description",
            Price = 200,
            CountInStock = 20,
            IsAvailable = true
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products/product", product);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ValidData_ReturnsNoContent()
    {
        // Arrange
        var updatedProduct = new ProductDTO
        {
            ProductName = _testProduct.ProductName,
            Description = "Updated Description",
            Price = 150,
            CountInStock = 15,
            IsAvailable = true
        };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products/product", updatedProduct);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ValidId_ReturnsNoContent()
    {
        // Act
        var response = await Client.DeleteAsync($"/api/products/product/{_testProduct.ProductName}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAllUserProducts_ReturnsNoContent()
    {
        // Act
        var response = await Client.DeleteAsync("/api/products/product/userProducts");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ReturnUserProducts_ReturnsNoContent()
    {
        // Act
        var response = await Client.PostAsync("/api/products/product/activation", null);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}