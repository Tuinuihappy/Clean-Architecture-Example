using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

/// <summary>
/// Category Entity — หมวดหมู่สินค้า
/// สืบทอดจาก AggregateRoot (Category ถือเป็นอีก Root หนึ่ง)
/// </summary>
public class Category : AggregateRoot
{
    // ลบ Id ออก เพราะ base class มีแล้ว
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation property
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    // EF Core constructor
    private Category() { }

    public Category(string name, string? description = null)
    {
        SetName(name);
        Description = description;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new Events.CategoryCreatedEvent(this));
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");

        if (name.Length > 100)
            throw new DomainException("Category name cannot exceed 100 characters.");

        if (Name != name.Trim())
        {
            Name = name.Trim();
            AddDomainEvent(new Events.CategoryUpdatedEvent(this));
        }
    }

    public void SetDescription(string? description)
    {
        var newDesc = description?.Trim();
        if (Description != newDesc)
        {
            Description = newDesc;
            AddDomainEvent(new Events.CategoryUpdatedEvent(this));
        }
    }

    public void RecordDeletion()
    {
        AddDomainEvent(new Events.CategoryDeletedEvent(this));
    }
}
