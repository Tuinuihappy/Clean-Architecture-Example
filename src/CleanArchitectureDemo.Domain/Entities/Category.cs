using CleanArchitectureDemo.Domain.Exceptions;

namespace CleanArchitectureDemo.Domain.Entities;

/// <summary>
/// Category Entity — หมวดหมู่สินค้า
/// Entity ใน Domain layer จะมี business logic อยู่ภายใน (Rich Domain Model)
/// </summary>
public class Category
{
    public int Id { get; private set; }
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
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");

        if (name.Length > 100)
            throw new DomainException("Category name cannot exceed 100 characters.");

        Name = name.Trim();
    }

    public void SetDescription(string? description)
    {
        Description = description?.Trim();
    }
}
