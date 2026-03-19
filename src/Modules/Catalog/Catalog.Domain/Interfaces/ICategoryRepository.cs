using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;

/// <summary>
/// Repository Interface สำหรับ Category
/// </summary>
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasProductsAsync(int categoryId);
}
