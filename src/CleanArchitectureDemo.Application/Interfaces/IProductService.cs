using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Application.DTOs;

namespace CleanArchitectureDemo.Application.Interfaces;

/// <summary>
/// Service Interface สำหรับ Product Use Cases
/// ประกาศไว้ใน Application layer — Implementation อยู่ใน Application layer เช่นกัน
/// (ต่างจาก Repository Interface ที่ประกาศใน Domain แต่ implement ใน Infrastructure)
/// </summary>
public interface IProductService
{
    Task<Result<ProductDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<ProductDto>>> GetAllAsync();
    Task<Result<IEnumerable<ProductDto>>> GetByCategoryIdAsync(int categoryId);
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result<ProductDto>> ActivateAsync(int id);
    Task<Result<ProductDto>> DeactivateAsync(int id);
}
