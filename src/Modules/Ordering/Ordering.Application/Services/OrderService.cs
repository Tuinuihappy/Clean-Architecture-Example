using CleanArchitectureDemo.Modules.Ordering.Domain.Entities;
using CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.IntegrationEvents;
using MediatR;

namespace CleanArchitectureDemo.Modules.Ordering.Application.Services;

public interface IOrderService
{
    Task<Result<int>> PlaceOrderAsync(int productId, string productName, decimal unitPrice, int quantity);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IPublisher _publisher;

    public OrderService(IOrderRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Result<int>> PlaceOrderAsync(int productId, string productName, decimal unitPrice, int quantity)
    {
        var order = new Order();
        order.AddItem(productId, productName, unitPrice, quantity);

        await _repository.AddAsync(order);

        // 2. สนทนาข้าม Module ด้วย Integration Event (EDA ข้าม Module)
        var purchasedItems = new Dictionary<int, int> { { productId, quantity } };
        var integrationEvent = new OrderCreatedIntegrationEvent(order.Id, order.TotalAmount, purchasedItems);
        
        // Publish ผ่าน MediatR (จะไปเข้า Handler ใน Catalog Module ทันที)
        await _publisher.Publish(integrationEvent);

        return Result<int>.Success(order.Id);
    }
}
