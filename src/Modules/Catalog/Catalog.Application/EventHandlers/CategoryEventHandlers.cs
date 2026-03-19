using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Modules.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDemo.Modules.Catalog.Application.EventHandlers;

public class CategoryCreatedEventHandler : INotificationHandler<DomainEventNotification<CategoryCreatedEvent>>
{
    private readonly ILogger<CategoryCreatedEventHandler> _logger;

    public CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<CategoryCreatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Category {Name} (ID: {Id}) was CREATED.", notification.DomainEvent.Category.Name, notification.DomainEvent.Category.Id);
        return Task.CompletedTask;
    }
}

public class CategoryUpdatedEventHandler : INotificationHandler<DomainEventNotification<CategoryUpdatedEvent>>
{
    private readonly ILogger<CategoryUpdatedEventHandler> _logger;

    public CategoryUpdatedEventHandler(ILogger<CategoryUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<CategoryUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Category {Name} (ID: {Id}) was UPDATED.", notification.DomainEvent.Category.Name, notification.DomainEvent.Category.Id);
        return Task.CompletedTask;
    }
}

public class CategoryDeletedEventHandler : INotificationHandler<DomainEventNotification<CategoryDeletedEvent>>
{
    private readonly ILogger<CategoryDeletedEventHandler> _logger;

    public CategoryDeletedEventHandler(ILogger<CategoryDeletedEventHandler> logger) => _logger = logger;

    public Task Handle(DomainEventNotification<CategoryDeletedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Category {Name} (ID: {Id}) was DELETED.", notification.DomainEvent.Category.Name, notification.DomainEvent.Category.Id);
        return Task.CompletedTask;
    }
}
