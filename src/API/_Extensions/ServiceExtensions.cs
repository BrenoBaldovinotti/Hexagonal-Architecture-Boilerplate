using Application.Commands;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
