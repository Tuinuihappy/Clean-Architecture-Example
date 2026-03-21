using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Products.Commands;

public record ActivateProductCommand(int ProductId) : ICommand;
public class ActivateProductCommandHandler : ICommandHandler<ActivateProductCommand>
{
    private readonly IProductRepository _repository;
    public ActivateProductCommandHandler(IProductRepository repository) => _repository = repository;
    public async Task<Result> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result.Failure("Product not found");
        product.Activate();
        await _repository.UpdateAsync(product);
        return Result.Success();
    }
}

public record DeactivateProductCommand(int ProductId) : ICommand;
public class DeactivateProductCommandHandler : ICommandHandler<DeactivateProductCommand>
{
    private readonly IProductRepository _repository;
    public DeactivateProductCommandHandler(IProductRepository repository) => _repository = repository;
    public async Task<Result> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result.Failure("Product not found");
        product.Deactivate();
        await _repository.UpdateAsync(product);
        return Result.Success();
    }
}

public record DiscontinueProductCommand(int ProductId) : ICommand;
public class DiscontinueProductCommandHandler : ICommandHandler<DiscontinueProductCommand>
{
    private readonly IProductRepository _repository;
    public DiscontinueProductCommandHandler(IProductRepository repository) => _repository = repository;
    public async Task<Result> Handle(DiscontinueProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result.Failure("Product not found");
        product.Discontinue();
        await _repository.UpdateAsync(product);
        return Result.Success();
    }
}
