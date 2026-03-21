using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDemo.Modules.Catalog.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core Configuration สำหรับ Product Entity
/// กำหนด schema, constraints, relationships ที่ database level
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.Status);

        // Seed data ตัวอย่าง
        builder.HasData(
            new { Id = 1, Name = "MacBook Pro M3", Description = "High performance laptop", Price = 1500.00m, StockQuantity = 10, CategoryId = 1, Status = CleanArchitectureDemo.Modules.Catalog.Domain.Enums.ProductStatus.Active, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, Name = "Clean Architecture T-Shirt", Description = "Cotton t-shirt with Uncle Bob quote", Price = 25.00m, StockQuantity = 50, CategoryId = 2, Status = CleanArchitectureDemo.Modules.Catalog.Domain.Enums.ProductStatus.Active, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, Name = "Domain-Driven Design", Description = "Tackling Complexity in the Heart of Software by Eric Evans", Price = 55.00m, StockQuantity = 100, CategoryId = 3, Status = CleanArchitectureDemo.Modules.Catalog.Domain.Enums.ProductStatus.Active, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
