using ClientsService.API;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using ClientsService.Persistence.Data;
using ClientsService.Persistence.Repositories;
using ClientsService.Persistence.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ClientsService.IntegrationTests;

public abstract class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly ClientsDbContext DbContext;

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
                    {"Jwt:Key", "your-super-secret-key-with-minimum-32-bytes-length-for-hs256"},
                    {"Jwt:Issuer", "test-issuer"},
                    {"Jwt:Audience", "test-audience"},
                    {"Jwt:ExpirationMinutes", "60"}
                });
            });

            builder.ConfigureServices(services =>
            {
                // Удаляем существующую регистрацию DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ClientsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Добавляем InMemory базу данных
                services.AddDbContext<ClientsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Регистрируем сервисы
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IPasswordService, PasswordService>();
                services.AddScoped<ITokenService, TokenService>();

                // Настраиваем авторизацию
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
                    options.AddPolicy("AllUsers", policy => policy.RequireRole("user", "admin"));
                });

                // Настраиваем аутентификацию
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "test-issuer",
                        ValidAudience = "test-audience",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("your-super-secret-key-with-minimum-32-bytes-length-for-hs256"))
                    };
                });
            });
        });

        Client = Factory.CreateClient();
        var scope = Factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ClientsDbContext>();
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    protected async Task<ClientsDbContext> GetDbContext()
    {
        return DbContext;
    }

    protected async Task CleanupAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
    }
} 