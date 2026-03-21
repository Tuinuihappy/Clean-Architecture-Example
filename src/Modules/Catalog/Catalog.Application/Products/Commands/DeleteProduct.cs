using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Products.Commands;

public record DeleteProductCommand(int ProductId) : ICommand;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result.Failure("Product not found");

        product.RecordDeletion();
        await _repository.DeleteAsync(product);
        return Result.Success();
    }
}
