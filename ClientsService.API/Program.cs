using ClientsService.API.Extentions;
using ClientsService.API.Middlewares;
using ClientsService.Application.DTOs.MappingProfiles;
using ClientsService.Application.Exceptions;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Persistence.Data;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureValidation();
builder.Services.ConfigureHttpClient(builder.Configuration);

//builder.Services.ConfigureRequestServices();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();
    
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClientsDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
//app.UseStaticFiles(); <--- Dont need it for now
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
