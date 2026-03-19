using CleanArchitectureDemo.Domain.Enums;

namespace CleanArchitectureDemo.Application.DTOs;

/// <summary>
/// DTO สำหรับส่ง Product data กลับไปยัง client
/// แยก DTO ออกจาก Entity เพื่อไม่ให้ expose domain model ตรงๆ
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO สำหรับสร้าง Product ใหม่
/// </summary>
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
}

/// <summary>
/// DTO สำหรับอัปเดต Product
/// </summary>
public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
}
