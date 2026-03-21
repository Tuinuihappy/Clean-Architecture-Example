using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Categories.Commands;

public record CreateCategoryCommand(string Name, string Description) : ICommand<int>;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _repository;
    public CreateCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;
    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name, request.Description);
        await _repository.AddAsync(category);
        return Result<int>.Success(category.Id);
    }
}

public record UpdateCategoryCommand(int CategoryId, string Name, string Description) : ICommand;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _repository;
    public UpdateCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId);
        if (category == null) return Result.Failure("Category not found");
        category.SetName(request.Name);
        category.SetDescription(request.Description);
        await _repository.UpdateAsync(category);
        return Result.Success();
    }
}

public record DeleteCategoryCommand(int CategoryId) : ICommand;
public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _repository;
    public DeleteCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId);
        if (category == null) return Result.Failure("Category not found");
        category.RecordDeletion();
        // assuming no sub-products logic is needed in Application (handled by Domain or DB)
        await _repository.DeleteAsync(category);
        return Result.Success();
    }
}
