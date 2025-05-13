using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductService.Application.Commands;
using ProductService.Application.DTOs;
using ProductService.Application.DTOs.MappingProfiles;
using ProductService.Application.Queries;
using ProductService.Domain.Interfaces;
using ProductService.Persistence.Data;
using ProductService.Persistence.Interceptors;
using ProductService.Persistence.Repositories;

namespace ProductsService.API.Extentions;

public static class ServiceExtentions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new NullReferenceException();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                };
                options.MapInboundClaims = false;
            });
        return services;
    }
    
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "InnoShop.Products", Version = "v1" });
            
            /*var xmlFile = $"{typeof(DeleteProductCommand).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);*/
        });
        
        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<IRequestHandler<AddProductCommand>, AddProductCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteProductCommand>, DeleteProductCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserProductsCommand>, DeleteUserProductsCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateProductCommand>, UpdateProductCommandHandler>();
        services.AddScoped<IRequestHandler<ReturnUserProductsCommand>, ReturnUserProductsCommandHandler>();
        
        services.AddScoped<IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>, GetAllProductsQueryHandler>();
        services
            .AddScoped<IRequestHandler<GetUserProductsQuery, IEnumerable<ProductDTO>>, GetUserProductsQueryHandler>();
        
        return services;
    }
    
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProductsDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
        });
        return services;
    }
    
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT-токен"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] {}
                }
            });
        });
        
        return services;
    }
    
    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
        var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        return services;
    }
    
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        return services;
    }
}