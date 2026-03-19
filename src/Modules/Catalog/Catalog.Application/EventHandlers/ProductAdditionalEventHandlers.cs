using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Modules.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDemo.Modules.Catalog.Application.EventHandlers;

public class ProductDeactivatedEventHandler : INotificationHandler<DomainEventNotification<ProductDeactivatedEvent>>
{
    private readonly ILogger<ProductDeactivatedEventHandler> _logger;

    public ProductDeactivatedEventHandler(ILogger<ProductDeactivatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductDeactivatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Product {Name} (ID: {Id}) was DEACTIVATED.", notification.DomainEvent.Product.Name, notification.DomainEvent.Product.Id);
        return Task.CompletedTask;
    }
}

public class ProductDiscontinuedEventHandler : INotificationHandler<DomainEventNotification<ProductDiscontinuedEvent>>
{
    private readonly ILogger<ProductDiscontinuedEventHandler> _logger;

    public ProductDiscontinuedEventHandler(ILogger<ProductDiscontinuedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductDiscontinuedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Domain Event: Product {Name} (ID: {Id}) was DISCONTINUED.", notification.DomainEvent.Product.Name, notification.DomainEvent.Product.Id);
        return Task.CompletedTask;
    }
}

public class ProductPriceChangedEventHandler : INotificationHandler<DomainEventNotification<ProductPriceChangedEvent>>
{
    private readonly ILogger<ProductPriceChangedEventHandler> _logger;

    public ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductPriceChangedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Product {Name} (ID: {Id}) changed price from {OldPrice} to {NewPrice}.", 
            notification.DomainEvent.Product.Name, 
            notification.DomainEvent.Product.Id,
            notification.DomainEvent.OldPrice,
            notification.DomainEvent.NewPrice);
        return Task.CompletedTask;
    }
}

public class ProductStockUpdatedEventHandler : INotificationHandler<DomainEventNotification<ProductStockUpdatedEvent>>
{
    private readonly ILogger<ProductStockUpdatedEventHandler> _logger;

    public ProductStockUpdatedEventHandler(ILogger<ProductStockUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductStockUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Product {Name} (ID: {Id}) changed stock from {OldStock} to {NewStock}.", 
            notification.DomainEvent.Product.Name, 
            notification.DomainEvent.Product.Id,
            notification.DomainEvent.OldQuantity,
            notification.DomainEvent.NewQuantity);
        return Task.CompletedTask;
    }
}

public class ProductUpdatedEventHandler : INotificationHandler<DomainEventNotification<ProductUpdatedEvent>>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Product {Name} (ID: {Id}) details were updated.", notification.DomainEvent.Product.Name, notification.DomainEvent.Product.Id);
        return Task.CompletedTask;
    }
}

public class ProductDeletedEventHandler : INotificationHandler<DomainEventNotification<ProductDeletedEvent>>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<ProductDeletedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Product {Name} (ID: {Id}) was DELETED.", notification.DomainEvent.Product.Name, notification.DomainEvent.Product.Id);
        return Task.CompletedTask;
    }
}
