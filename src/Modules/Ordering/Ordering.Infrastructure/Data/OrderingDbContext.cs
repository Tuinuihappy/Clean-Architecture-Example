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

        modelBuilder.Entity<Order>().HasData(
            new { Id = 1, OrderDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1525.00m },
            new { Id = 2, OrderDate = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 55.00m }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new { Id = 1, ProductId = 1, ProductName = "MacBook Pro M3", UnitPrice = 1500.00m, Quantity = 1, OrderId = 1 },
            new { Id = 2, ProductId = 2, ProductName = "Clean Architecture T-Shirt", UnitPrice = 25.00m, Quantity = 1, OrderId = 1 },
            new { Id = 3, ProductId = 3, ProductName = "Domain-Driven Design", UnitPrice = 55.00m, Quantity = 1, OrderId = 2 }
        );
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
