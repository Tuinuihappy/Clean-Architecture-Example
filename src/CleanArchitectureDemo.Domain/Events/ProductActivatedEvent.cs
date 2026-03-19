using CleanArchitectureDemo.Domain.Common;
using CleanArchitectureDemo.Domain.Entities;

namespace CleanArchitectureDemo.Domain.Events;

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
