using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Domain.Common;
using CleanArchitectureDemo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDemo.Infrastructure.Data;

/// <summary>
/// EF Core DbContext — จัดการการเชื่อมต่อ Database
/// อยู่ใน Infrastructure layer เพราะเป็น external concern
/// </summary>
public class AppDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 1. Dispatch Domain Events ก่อน Save
        await DispatchDomainEventsAsync();

        // 2. Commit ลง DB
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        // หา AggregateRoot ทั้งหมดที่มี Domain Events ค้างอยู่
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        // รวม Events ทั้งหมด
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // เคลียร์ Events ทิ้งเพื่อไม่ให้วนลูป
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        // Dispatch แต่ละ Event ให้ MediatR นำไปให้ Handler (Application Layer)
        foreach (var domainEvent in domainEvents)
        {
            // แปลง IDomainEvent ให้เป็น INotification ของ MediatR ผ่าน wrapper
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;

            await _publisher.Publish(notification);
        }
    }
}
