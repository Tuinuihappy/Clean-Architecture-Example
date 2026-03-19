using CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Data;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Repositories;
using CleanArchitectureDemo.Modules.Ordering.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDemo.Modules.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services)
    {
        services.AddOrderingApplication();

        // Initialize OrderingDbContext independent of the Catalog module
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseInMemoryDatabase("OrderingDb"));

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
