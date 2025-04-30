using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using ClientsService.API.Validators;
using ClientsService.Application.Clients;
using ClientsService.Application.Clients.Interfaces;
using ClientsService.Application.Commands;
using ClientsService.Application.DTOs;
using ClientsService.Application.DTOs.MappingProfiles;
using ClientsService.Application.Queries;
using ClientsService.Application.Services.Interfaces;
using ClientsService.Domain.Interfaces;
using ClientsService.Persistence.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ClientsService.Persistence.Repositories;
using ClientsService.Persistence.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ClientsService.API.Extentions;

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "InnoShop.Clients", Version = "v1" });
            
            var xmlFile = $"{typeof(DeleteUserCommand).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        
        return services;
    }
    
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
    
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ClientsDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
    
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            options.AddPolicy("AllUsers", policy => policy.RequireRole("user", "admin"));
        });
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

    public static IServiceCollection ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var productUri = configuration["Uri:ProductService"] ?? throw new NullReferenceException();
        services.AddHttpClient<ProductServiceClient>(client =>
        {
            client.BaseAddress = new Uri(productUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        return services;
    }
    
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        //services.AddScoped<IProductServiceClient, ProductServiceClient>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddScoped<IRequestHandler<ActivateUserCommand>, ActivateUserCommandHandler>();
        services.AddScoped<IRequestHandler<DeactivateUserCommand>, DeactivateUserCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        services.AddScoped<IRequestHandler<LoginUserCommand, TokenDTO>, LoginUserCommandHandler>();
        services.AddScoped<IRequestHandler<RegisterUserCommand, TokenDTO>, RegisterUserCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
        services.AddScoped<IRequestHandler<GetUserByEmailQuery, UserDTO>, GetUserByEmailQueryHandler>();
        services.AddScoped<IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>, GetUsersQueryHandler>();

        return services;
    }
    
    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        return services;
    }
}