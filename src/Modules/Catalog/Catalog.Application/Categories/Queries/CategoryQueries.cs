using CleanArchitectureDemo.Modules.Catalog.Application.DTOs;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Categories.Queries;

public record GetCategoryByIdQuery(int CategoryId) : IQuery<CategoryDto>;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryByIdQueryHandler(ICategoryRepository repository) => _repository = repository;

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId);
        if (category == null) return Result<CategoryDto>.Failure("Category not found");

        var dto = new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description };
        return Result<CategoryDto>.Success(dto);
    }
}

public record GetCategoriesQuery() : IQuery<IEnumerable<CategoryDto>>;

public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoriesQueryHandler(ICategoryRepository repository) => _repository = repository;

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync();
        var dtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description });
        return Result<IEnumerable<CategoryDto>>.Success(dtos);
    }
}
