using MediatR;

namespace CleanArchitectureDemo.Shared.Kernel.IntegrationEvents;

/// <summary>
/// Integration Event ใช้สำหรับสื่อสารข้าม Module 
/// (เช่น ซื้อของเสร็จให้แจ้ง Catalog Module ลดสต๊อก)
/// </summary>
public class OrderCreatedIntegrationEvent : INotification
{
    public int OrderId { get; }
    public decimal TotalAmount { get; }
    public IReadOnlyDictionary<int, int> PurchasedItems { get; } // ProductId -> Quantity

    public OrderCreatedIntegrationEvent(int orderId, decimal totalAmount, Dictionary<int, int> purchasedItems)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
        PurchasedItems = purchasedItems;
    }
}
