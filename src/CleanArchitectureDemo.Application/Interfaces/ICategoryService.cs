using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Application.DTOs;

namespace CleanArchitectureDemo.Application.Interfaces;

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
