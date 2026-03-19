using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Application.Services;
using CleanArchitectureDemo.Domain.Interfaces;
using CleanArchitectureDemo.Infrastructure.Data;
using CleanArchitectureDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDemo.Infrastructure;

/// <summary>
/// Extension method สำหรับลงทะเบียน Dependencies ของ Infrastructure layer
/// ช่วยให้ API layer ไม่ต้องรู้รายละเอียดภายในของ Infrastructure
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
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
