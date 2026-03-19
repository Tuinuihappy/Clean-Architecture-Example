using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Modules.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDemo.Modules.Catalog.Infrastructure.Repositories;

/// <summary>
/// Product Repository Implementation
/// Implement IProductRepository ที่ประกาศไว้ใน Domain layer
/// นี่คือตัวอย่างของ Dependency Inversion Principle:
///   - Domain layer กำหนด interface (abstraction)
///   - Infrastructure layer implement interface นั้น (detail)
///   - Domain ไม่ขึ้นกับ Infrastructure แต่ Infrastructure ขึ้นกับ Domain
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Reload with navigation property
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}
