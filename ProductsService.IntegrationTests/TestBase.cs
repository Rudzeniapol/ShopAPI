using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ProductService.Domain.Interfaces;
using ProductService.Persistence.Data;
using ProductService.Persistence.Repositories;
using ProductsService.API;

namespace ProductsService.IntegrationTests;

public abstract class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly ProductsDbContext DbContext;

    protected TestBase(WebApplicationFactory<Program> factory)
    {
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:DefaultConnection", "TestDb"},
                    {"Jwt:Key", "super_mega_secret_key_1234567890"},
                    {"Jwt:Issuer", "InnoShop"},
                    {"Jwt:Audience", "Clients"},
                    {"Jwt:ExpirationMinutes", "60"}
                });
            });

            builder.ConfigureServices(services =>
            {
                // Удаляем существующую регистрацию DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProductsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Добавляем InMemory базу данных
                services.AddDbContext<ProductsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Регистрируем сервисы
                services.AddScoped<IProductRepository, ProductRepository>();
            });
        });

        Client = Factory.CreateClient();
        var scope = Factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    protected async Task<ProductsDbContext> GetDbContext()
    {
        return DbContext;
    }

    protected async Task CleanupAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
    }
} 