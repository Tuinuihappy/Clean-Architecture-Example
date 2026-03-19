using CleanArchitectureDemo.Modules.Catalog.Application.Interfaces;
using CleanArchitectureDemo.Modules.Catalog.Application.Services;
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
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        // ลงทะเบียน Application Layer ภายใน Module นี้
        services.AddApplication();

        // Register DbContext (ใช้ InMemory สำหรับ demo)
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("CleanArchitectureDb"));

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Register Application Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
