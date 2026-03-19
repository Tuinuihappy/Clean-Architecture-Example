using CleanArchitectureDemo.Domain.Entities;

namespace CleanArchitectureDemo.Domain.Interfaces;

/// <summary>
/// Repository Interface สำหรับ Product
/// ประกาศไว้ใน Domain layer (Dependency Inversion Principle)
/// Implementation จริงอยู่ใน Infrastructure layer
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> ExistsAsync(int id);
}
