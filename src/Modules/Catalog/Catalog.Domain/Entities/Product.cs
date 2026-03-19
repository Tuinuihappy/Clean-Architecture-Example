using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Enums;
using CleanArchitectureDemo.Modules.Catalog.Domain.Events;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

/// <summary>
/// Product Entity — สินค้า
/// สืบทอดจาก AggregateRoot ทำให้สามารถบันทึก Domain Events ภายในตัวเองได้
/// </summary>
public class Product : AggregateRoot
{
    // ลบ Id ออก เพราะ base class มีแล้ว
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

        // เมื่อสร้าง Product ให้ raise event สร้างเสร็จแล้วด้วย
        AddDomainEvent(new ProductCreatedEvent(this));
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
        AddDomainEvent(new ProductUpdatedEvent(this));
    }

    public void SetDescription(string? description)
    {
        if (Description != description?.Trim())
        {
            Description = description?.Trim();
            MarkUpdated();
            AddDomainEvent(new ProductUpdatedEvent(this));
        }
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        if (Price != price)
        {
            var oldPrice = Price;
            Price = price;
            MarkUpdated();
            AddDomainEvent(new ProductPriceChangedEvent(this, oldPrice, price));
        }
    }

    public void SetStockQuantity(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        if (StockQuantity != quantity)
        {
            var oldQuantity = StockQuantity;
            StockQuantity = quantity;
            MarkUpdated();
            AddDomainEvent(new ProductStockUpdatedEvent(this, oldQuantity, quantity));
        }
    }

    public void SetCategory(int categoryId)
    {
        if (categoryId <= 0)
            throw new DomainException("Invalid category.");

        if (CategoryId != categoryId)
        {
            CategoryId = categoryId;
            MarkUpdated();
            AddDomainEvent(new ProductUpdatedEvent(this));
        }
    }

    public void Activate()
    {
        if (Price <= 0)
            throw new DomainException("Cannot activate a product with zero or negative price.");

        Status = ProductStatus.Active;
        MarkUpdated();

        // เมื่อ Product ถูก Activate ให้ raise event ด้วย
        AddDomainEvent(new ProductActivatedEvent(this));
    }

    public void Deactivate()
    {
        if (Status != ProductStatus.Inactive)
        {
            Status = ProductStatus.Inactive;
            MarkUpdated();
            AddDomainEvent(new ProductDeactivatedEvent(this));
        }
    }

    public void Discontinue()
    {
        if (Status != ProductStatus.Discontinued)
        {
            Status = ProductStatus.Discontinued;
            MarkUpdated();
            AddDomainEvent(new ProductDiscontinuedEvent(this));
        }
    }

    public void RecordDeletion()
    {
        AddDomainEvent(new ProductDeletedEvent(this));
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
