using CleanArchitectureDemo.Shared.Kernel.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDemo.Modules.Catalog.Application.EventHandlers;

/// <summary>
/// Handler ภายใน Catalog Module เพื่อรับฟัง Integration Event จาก Ordering Module
/// </summary>
public class OrderCreatedIntegrationEventHandler : INotificationHandler<OrderCreatedIntegrationEvent>
{
    private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;
    // Inject IProductRepository เพื่อตัดสต๊อกจริงได้ด้วย

    public OrderCreatedIntegrationEventHandler(ILogger<OrderCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("📦 [Catalog Module] Received Integration Event from Ordering!");
        _logger.LogWarning("   -> Order #{OrderId} placed with total {Amount:C}", notification.OrderId, notification.TotalAmount);
        
        foreach (var item in notification.PurchasedItems)
        {
            _logger.LogWarning("   -> Deducting stock for Product ID: {ProductId}, Qty: {Qty}", item.Key, item.Value);
            // โค้ดตัดสต๊อกจริงจะอยู่ตรงนี้:
            // var product = await _productRepository.GetByIdAsync(item.Key);
            // product.SetStockQuantity(product.StockQuantity - item.Value);
            // await _productRepository.UpdateAsync(product);
        }

        return Task.CompletedTask;
    }
}
