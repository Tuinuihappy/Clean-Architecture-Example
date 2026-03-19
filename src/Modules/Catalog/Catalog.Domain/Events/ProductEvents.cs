using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Catalog.Domain.Events;

public class ProductDeactivatedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductDeactivatedEvent(Product product)
    {
        Product = product;
    }
}

public class ProductDiscontinuedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductDiscontinuedEvent(Product product)
    {
        Product = product;
    }
}

public class ProductPriceChangedEvent : IDomainEvent
{
    public Product Product { get; }
    public decimal OldPrice { get; }
    public decimal NewPrice { get; }

    public ProductPriceChangedEvent(Product product, decimal oldPrice, decimal newPrice)
    {
        Product = product;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}

public class ProductStockUpdatedEvent : IDomainEvent
{
    public Product Product { get; }
    public int OldQuantity { get; }
    public int NewQuantity { get; }

    public ProductStockUpdatedEvent(Product product, int oldQuantity, int newQuantity)
    {
        Product = product;
        OldQuantity = oldQuantity;
        NewQuantity = newQuantity;
    }
}

public class ProductUpdatedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }
}

public class ProductDeletedEvent : IDomainEvent
{
    public Product Product { get; }

    public ProductDeletedEvent(Product product)
    {
        Product = product;
    }
}
