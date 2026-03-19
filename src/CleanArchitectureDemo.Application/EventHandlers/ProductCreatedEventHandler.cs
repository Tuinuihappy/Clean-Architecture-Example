using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDemo.Application.EventHandlers;

/// <summary>
/// Event Handler สำหรับรับ Event เมื่อมีสินค้าถูกสร้างใหม่
/// (ในโปรเจคจริง อาจใช้ส่ง Email, แจ้งเตือนผ่าน Slack, หรือส่ง msg ไปยัง Service อื่น)
/// </summary>
public class ProductCreatedEventHandler : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Domain Event Handled: Product {ProductName} (ID: {ProductId}) was created at {CreatedAt}.",
            domainEvent.Product.Name,
            domainEvent.Product.Id,
            domainEvent.Product.CreatedAt);

        return Task.CompletedTask;
    }
}
