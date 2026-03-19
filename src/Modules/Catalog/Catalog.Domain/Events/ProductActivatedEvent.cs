using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Events;

/// <summary>
/// Domain Event: ยืนยันว่า Product ถูกนำกลับมาเปิดใช้งาน (Activated)
/// </summary>
public class ProductActivatedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductActivatedEvent(Product product)
    {
        Product = product;
    }
}
