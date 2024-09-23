using Domain.Entities;
using Domain.Services;
using Domain.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Swashbuckle;
using Microsoft.OpenApi.Models;

namespace API._Extensions;

public static class ServiceExtensions
{
    // Extension method to configure Serilog
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        return services;
    }

    // Extension method to add JWT Authentication
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, string? jwtSecret)
    {
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new ArgumentNullException(nameof(jwtSecret), "JWT Secret key is not configured.");
        }

        var key = Encoding.ASCII.GetBytes(jwtSecret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Hexagonal Architecture API",
                Description = "API Documentation for the Hexagonal Architecture Boilerplate",
                Contact = new OpenApiContact
                {
                    Name = "Breno Baldovinotti",
                    Email = "brenobaldovinotti@gmail.com"
                }
            });
        });

        return services;
    }

    // Add FluentValidation and Validators
    public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddScoped<IValidator<Order>, OrderValidator>();
        services.AddScoped<IValidator<OrderItem>, OrderItemValidator>();

        return services;
    }

    // Extension method to add CORS
    public static IServiceCollection AddCustomCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
        });

        return services;
    }

    // Extension method to add memory caching
    public static IServiceCollection AddCustomCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }

    // Extension method to configure MVC Controllers
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    // Register MediatR and CQRS Handlers
    public static IServiceCollection AddCustomCQRS(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }

    // Register Domain Services
    public static IServiceCollection AddCustomDomainServices(this IServiceCollection services)
    {
        services.AddScoped<OrderService>();

        return services;
    }
}
