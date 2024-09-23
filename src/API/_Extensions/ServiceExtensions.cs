using Domain.Entities;
using Domain.Services;
using Domain.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;
using Application.Security.AuthorizationHandlers;
using Application.Security.Requirements;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace API._Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["Jwt:Key"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtSecret) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new ArgumentNullException(nameof(jwtSecret), "JWT is not configured.");
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
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

        services.AddAuthorizationBuilder().AddPolicy("AdminPolicy", policy => policy.Requirements.Add(new RoleRequirement("Admin")));

        return services;
    }

    public static IServiceCollection AddCustomVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
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

    public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<Order>, OrderValidator>();
        services.AddScoped<IValidator<OrderItem>, OrderItemValidator>();

        return services;
    }

    public static IServiceCollection AddCustomCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    public static IServiceCollection AddCustomCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }

    public static IServiceCollection AddCustomCQRS(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        return services;
    }

    public static IServiceCollection AddCustomDomainServices(this IServiceCollection services)
    {
        services.AddScoped<OrderService>();
        return services;
    }
}
