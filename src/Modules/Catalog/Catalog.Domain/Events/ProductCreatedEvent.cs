using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Events;

/// <summary>
/// Domain Event: ยืนยันว่า Product ใหม่ถูกสร้างขึ้น
/// </summary>
public class ProductCreatedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }
}
