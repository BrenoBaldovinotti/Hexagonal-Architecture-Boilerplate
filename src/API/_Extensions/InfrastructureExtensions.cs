using Domain.Repositories;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API._Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddCustomInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the DbContext with PostgreSQL connection
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
