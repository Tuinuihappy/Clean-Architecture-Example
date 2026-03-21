// Removed application services using statements
using Microsoft.Extensions.Configuration;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Modules.Catalog.Infrastructure.Data;
using CleanArchitectureDemo.Modules.Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureDemo.Modules.Catalog.Application;

namespace CleanArchitectureDemo.Modules.Catalog.Infrastructure;

/// <summary>
/// Extension method สำหรับลงทะเบียน Dependencies ของ Catalog Module
/// ช่วยให้ Host (API layer) ไม่ต้องรู้รายละเอียดภายในของ Module
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // ลงทะเบียน Application Layer ภายใน Module นี้
        services.AddApplication();

        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CatalogDbConnection")));

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Note: Features are now registered via MediatR inside AddApplication()

        return services;
    }
}
