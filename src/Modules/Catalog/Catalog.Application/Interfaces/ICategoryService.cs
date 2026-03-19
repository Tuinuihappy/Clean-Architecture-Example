using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Modules.Catalog.Application.DTOs;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Interfaces;

/// <summary>
/// Service Interface สำหรับ Category Use Cases
/// </summary>
public interface ICategoryService
{
    Task<Result<CategoryDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<Result> DeleteAsync(int id);
}
