using CleanArchitectureDemo.Modules.Ordering.Domain.Entities;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDemo.Modules.Ordering.Infrastructure.Data;

public class OrderingDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public OrderingDbContext(DbContextOptions<OrderingDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;
            await _publisher.Publish(notification);
        }
    }
}
