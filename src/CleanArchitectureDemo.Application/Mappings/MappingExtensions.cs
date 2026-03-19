using CleanArchitectureDemo.Domain.Entities;

namespace CleanArchitectureDemo.Application.Mappings;

/// <summary>
/// Extension methods สำหรับ mapping ระหว่าง Entity กับ DTO
/// ใช้ manual mapping เพื่อความเรียบง่าย (สามารถเปลี่ยนเป็น AutoMapper ได้)
/// </summary>
public static class MappingExtensions
{
    // ===== Product Mappings =====

    public static DTOs.ProductDto ToDto(this Product product)
    {
        return new DTOs.ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Status = product.Status.ToString(),
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public static IEnumerable<DTOs.ProductDto> ToDto(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }

    // ===== Category Mappings =====

    public static DTOs.CategoryDto ToDto(this Category category)
    {
        return new DTOs.CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            ProductCount = category.Products?.Count ?? 0
        };
    }

    public static IEnumerable<DTOs.CategoryDto> ToDto(this IEnumerable<Category> categories)
    {
        return categories.Select(c => c.ToDto());
    }
}
