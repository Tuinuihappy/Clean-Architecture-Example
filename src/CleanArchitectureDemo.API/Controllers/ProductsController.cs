using CleanArchitectureDemo.Modules.Catalog.Application.DTOs;
using CleanArchitectureDemo.Modules.Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers;

/// <summary>
/// Products API Controller
/// Controller ทำหน้าที่รับ HTTP request แล้วส่งต่อไปให้ Service (Application layer)
/// ไม่มี business logic ใน Controller — ทำแค่ mapping HTTP → Service call → HTTP response
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// ดึงสินค้าทั้งหมด
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// ดึงสินค้าตาม ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// ดึงสินค้าตามหมวดหมู่
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _productService.GetByCategoryIdAsync(categoryId);
        if (!result.IsSuccess)
            return NotFound(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// สร้างสินค้าใหม่
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// อัปเดตสินค้า
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var result = await _productService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// ลบสินค้า
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result.IsSuccess)
            return NotFound(new { message = result.ErrorMessage });

        return NoContent();
    }

    /// <summary>
    /// เปิดใช้งานสินค้า (Activate)
    /// </summary>
    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var result = await _productService.ActivateAsync(id);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// ปิดใช้งานสินค้า (Deactivate)
    /// </summary>
    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result = await _productService.DeactivateAsync(id);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }
}
