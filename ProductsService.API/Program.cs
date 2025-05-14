using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Persistence.Data;
using ProductsService.API.Extentions;
using ProductsService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Проверяем, находимся ли мы в тестовом окружении
var isTestEnvironment = builder.Environment.EnvironmentName == "Testing";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureCors();

// Настраиваем контекст базы данных
if (isTestEnvironment)
{
    builder.Services.AddDbContext<ProductsDbContext>(options =>
    {
        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
    });
}
else
{
    builder.Services.ConfigureDatabaseContext(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Применяем миграции только если не в тестовом окружении
if (!isTestEnvironment)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    dbContext.Database.Migrate();
}

app.Run();

public partial class Program { } 
