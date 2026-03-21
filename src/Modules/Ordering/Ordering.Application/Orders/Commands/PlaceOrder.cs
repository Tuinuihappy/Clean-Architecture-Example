using CleanArchitectureDemo.Modules.Ordering.Domain.Entities;
using CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;
using MediatR;

namespace CleanArchitectureDemo.Modules.Ordering.Application.Orders.Commands;

public record PlaceOrderCommand(int ProductId, string ProductName, decimal UnitPrice, int Quantity) : ICommand<int>;

public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, int>
{
    private readonly IOrderRepository _repository;
    private readonly IPublisher _publisher;

    public PlaceOrderCommandHandler(IOrderRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Result<int>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();
        order.AddItem(request.ProductId, request.ProductName, request.UnitPrice, request.Quantity);

        await _repository.AddAsync(order);

        // Publish Cross-Module Integration Event via MediatR
        var purchasedItems = order.Items.ToDictionary(i => i.ProductId, i => i.Quantity);
        var integrationEvent = new CleanArchitectureDemo.Shared.Kernel.IntegrationEvents.OrderCreatedIntegrationEvent(
            order.Id, 
            order.TotalAmount, 
            purchasedItems);
            
        await _publisher.Publish(integrationEvent, cancellationToken);

        return Result<int>.Success(order.Id);
    }
}
