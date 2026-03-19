using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDemo.Modules.Catalog.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core Configuration สำหรับ Category Entity
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // Seed data ตัวอย่าง
        builder.HasData(
            new { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, Name = "Clothing", Description = "Apparel and fashion items", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, Name = "Books", Description = "Physical and digital books", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
