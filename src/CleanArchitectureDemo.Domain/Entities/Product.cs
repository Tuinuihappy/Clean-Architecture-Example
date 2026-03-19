using CleanArchitectureDemo.Domain.Enums;
using CleanArchitectureDemo.Domain.Exceptions;

namespace CleanArchitectureDemo.Domain.Entities;

/// <summary>
/// Product Entity — สินค้า
/// Rich Domain Model: Entity มี business logic ภายในตัว ไม่ใช่แค่ data container
/// ใช้ private set เพื่อบังคับให้แก้ไขข้อมูลผ่าน method ที่มี validation
/// </summary>
public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public ProductStatus Status { get; private set; }
    public int CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public Category Category { get; private set; } = null!;

    // EF Core constructor
    private Product() { }

    public Product(string name, string? description, decimal price, int stockQuantity, int categoryId)
    {
        SetName(name);
        Description = description;
        SetPrice(price);
        SetStockQuantity(stockQuantity);
        CategoryId = categoryId;
        Status = ProductStatus.Draft;
        CreatedAt = DateTime.UtcNow;
    }

    // ===== Business Logic Methods =====

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");

        if (name.Length > 200)
            throw new DomainException("Product name cannot exceed 200 characters.");

        Name = name.Trim();
        MarkUpdated();
    }

    public void SetDescription(string? description)
    {
        Description = description?.Trim();
        MarkUpdated();
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        Price = price;
        MarkUpdated();
    }

    public void SetStockQuantity(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        StockQuantity = quantity;
        MarkUpdated();
    }

    public void SetCategory(int categoryId)
    {
        if (categoryId <= 0)
            throw new DomainException("Invalid category.");

        CategoryId = categoryId;
        MarkUpdated();
    }

    public void Activate()
    {
        if (Price <= 0)
            throw new DomainException("Cannot activate a product with zero or negative price.");

        Status = ProductStatus.Active;
        MarkUpdated();
    }

    public void Deactivate()
    {
        Status = ProductStatus.Inactive;
        MarkUpdated();
    }

    public void Discontinue()
    {
        Status = ProductStatus.Discontinued;
        MarkUpdated();
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
