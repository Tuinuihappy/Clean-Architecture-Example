using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Events;

public class CategoryCreatedEvent : IDomainEvent
{
    public Category Category { get; }

    public CategoryCreatedEvent(Category category)
    {
        Category = category;
    }
}

public class CategoryUpdatedEvent : IDomainEvent
{
    public Category Category { get; }

    public CategoryUpdatedEvent(Category category)
    {
        Category = category;
    }
}

public class CategoryDeletedEvent : IDomainEvent
{
    public Category Category { get; }

    public CategoryDeletedEvent(Category category)
    {
        Category = category;
    }
}
