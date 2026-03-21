using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Products.Commands;

public record UpdateProductDetailsCommand(int ProductId, string NewName, int? CategoryId) : ICommand;

public class UpdateProductDetailsCommandHandler : ICommandHandler<UpdateProductDetailsCommand>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductDetailsCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result.Failure("Product not found");

        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
            if (category == null) return Result.Failure("Category not found");
        }

        product.SetName(request.NewName);
        if (request.CategoryId.HasValue)
        {
            product.SetCategory(request.CategoryId.Value);
        }
        await _repository.UpdateAsync(product);
        return Result.Success();
    }
}
