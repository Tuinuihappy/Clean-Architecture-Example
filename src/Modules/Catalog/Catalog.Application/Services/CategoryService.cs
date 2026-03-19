using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Modules.Catalog.Application.DTOs;
using CleanArchitectureDemo.Modules.Catalog.Application.Interfaces;
using CleanArchitectureDemo.Modules.Catalog.Application.Mappings;
using CleanArchitectureDemo.Modules.Catalog.Domain.Entities;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Services;

/// <summary>
/// Category Service — Use Cases สำหรับ Category
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            return Result<CategoryDto>.Failure($"Category with Id {id} not found.");

        return Result<CategoryDto>.Success(category.ToDto());
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Result<IEnumerable<CategoryDto>>.Success(categories.ToDto());
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        try
        {
            var category = new Category(dto.Name, dto.Description);
            var created = await _categoryRepository.AddAsync(category);
            return Result<CategoryDto>.Success(created.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<CategoryDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return Result<CategoryDto>.Failure($"Category with Id {id} not found.");

            category.SetName(dto.Name);
            category.SetDescription(dto.Description);

            await _categoryRepository.UpdateAsync(category);
            return Result<CategoryDto>.Success(category.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<CategoryDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            return Result.Failure($"Category with Id {id} not found.");

        if (await _categoryRepository.HasProductsAsync(id))
            return Result.Failure("Cannot delete category that has products.");

        category.RecordDeletion();
        await _categoryRepository.DeleteAsync(category);
        return Result.Success();
    }
}
