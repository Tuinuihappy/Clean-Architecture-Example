using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Products.Commands;

public record CreateProductCommand(string Name, string? Description, decimal Price, int StockQuantity, int CategoryId) : ICommand<int>;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Description, request.Price, request.StockQuantity, request.CategoryId);
        await _repository.AddAsync(product);
        return Result<int>.Success(product.Id);
    }
}
