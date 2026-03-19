using CleanArchitectureDemo.Domain.Common;
using CleanArchitectureDemo.Domain.Entities;

namespace CleanArchitectureDemo.Domain.Events;

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
