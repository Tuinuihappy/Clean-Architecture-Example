using CleanArchitectureDemo.Application.Common;
using CleanArchitectureDemo.Application.DTOs;
using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Application.Mappings;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Domain.Exceptions;
using CleanArchitectureDemo.Domain.Interfaces;

namespace CleanArchitectureDemo.Application.Services;

/// <summary>
/// Product Service — Use Cases / Application Logic
/// 
/// Service layer ทำหน้าที่เป็น orchestrator:
/// 1. รับ DTO จาก Controller
/// 2. เรียกใช้ Domain Entity เพื่อทำ business logic
/// 3. เรียกใช้ Repository เพื่อ persist data
/// 4. แปลง Entity กลับเป็น DTO แล้วส่งคืน
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return Result<ProductDto>.Failure($"Product with Id {id} not found.");

        return Result<ProductDto>.Success(product.ToDto());
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return Result<IEnumerable<ProductDto>>.Success(products.ToDto());
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetByCategoryIdAsync(int categoryId)
    {
        if (!await _categoryRepository.ExistsAsync(categoryId))
            return Result<IEnumerable<ProductDto>>.Failure($"Category with Id {categoryId} not found.");

        var products = await _productRepository.GetByCategoryIdAsync(categoryId);
        return Result<IEnumerable<ProductDto>>.Success(products.ToDto());
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        try
        {
            // ตรวจสอบว่า Category มีอยู่จริง
            if (!await _categoryRepository.ExistsAsync(dto.CategoryId))
                return Result<ProductDto>.Failure($"Category with Id {dto.CategoryId} not found.");

            // สร้าง Entity — business validation อยู่ใน constructor ของ Entity
            var product = new Product(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.StockQuantity,
                dto.CategoryId
            );

            var created = await _productRepository.AddAsync(product);
            return Result<ProductDto>.Success(created.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<ProductDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result<ProductDto>.Failure($"Product with Id {id} not found.");

            if (!await _categoryRepository.ExistsAsync(dto.CategoryId))
                return Result<ProductDto>.Failure($"Category with Id {dto.CategoryId} not found.");

            // อัปเดตผ่าน Domain method — มี validation อยู่ข้างใน
            product.SetName(dto.Name);
            product.SetDescription(dto.Description);
            product.SetPrice(dto.Price);
            product.SetStockQuantity(dto.StockQuantity);
            product.SetCategory(dto.CategoryId);

            await _productRepository.UpdateAsync(product);
            return Result<ProductDto>.Success(product.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<ProductDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return Result.Failure($"Product with Id {id} not found.");

        product.RecordDeletion();
        await _productRepository.DeleteAsync(product);
        return Result.Success();
    }

    public async Task<Result<ProductDto>> ActivateAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result<ProductDto>.Failure($"Product with Id {id} not found.");

            product.Activate();
            await _productRepository.UpdateAsync(product);
            return Result<ProductDto>.Success(product.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<ProductDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ProductDto>> DeactivateAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result<ProductDto>.Failure($"Product with Id {id} not found.");

            product.Deactivate();
            await _productRepository.UpdateAsync(product);
            return Result<ProductDto>.Success(product.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<ProductDto>.Failure(ex.Message);
        }
    }
}
