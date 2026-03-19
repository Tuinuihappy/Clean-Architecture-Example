using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

namespace CleanArchitectureDemo.Modules.Ordering.Domain.Entities;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount { get; private set; }

    public Order()
    {
        OrderDate = DateTime.UtcNow;
    }

    public void AddItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        var item = new OrderItem(productId, productName, unitPrice, quantity);
        _items.Add(item);
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        TotalAmount = _items.Sum(x => x.UnitPrice * x.Quantity);
    }
}
