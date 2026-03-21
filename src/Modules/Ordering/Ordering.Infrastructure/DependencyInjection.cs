using CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Data;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Repositories;
using CleanArchitectureDemo.Modules.Ordering.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CleanArchitectureDemo.Modules.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOrderingApplication();

        // Initialize OrderingDbContext independent of the Catalog module
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("OrderingDbConnection")));

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
