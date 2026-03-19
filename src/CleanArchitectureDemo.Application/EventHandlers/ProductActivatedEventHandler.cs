using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDemo.Application.EventHandlers;

/// <summary>
/// Event Handler สำหรับรับ Event เมื่อสินค้าถูกเรียกใช้งาน (Activate)
/// </summary>
public class ProductActivatedEventHandler : INotificationHandler<DomainEventNotification<ProductActivatedEvent>>
{
    private readonly ILogger<ProductActivatedEventHandler> _logger;

    public ProductActivatedEventHandler(ILogger<ProductActivatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProductActivatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogWarning("Domain Event Handled: Product {ProductName} (ID: {ProductId}) has been ACTIVATED. Sending notification email to admin...",
            domainEvent.Product.Name,
            domainEvent.Product.Id);

        // จำลองการใช้เวลาส่ง Email
        // await Task.Delay(1000, cancellationToken); 

        return Task.CompletedTask;
    }
}
