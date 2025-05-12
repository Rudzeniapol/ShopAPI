using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Persistence.Data;
using ProductsService.API.Extentions;
using ProductsService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureAutoMapper();
//builder.Services.ConfigureValidation();
//builder.Services.ConfigureRequestServices();
builder.Services.ConfigureCors();
builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();
    
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionMiddleware>();

//app.UseStaticFiles(); <--- Dont need it for now
app.UseAuthentication();
app.UseAuthorization();

//app.UseHttpsRedirection();

app.MapControllers();
app.Run();