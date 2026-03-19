using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

namespace CleanArchitectureDemo.Modules.Ordering.Domain.Entities;

public class OrderItem : Entity
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    // Required by EF Core
    private OrderItem() { ProductName = string.Empty; }

    public OrderItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
