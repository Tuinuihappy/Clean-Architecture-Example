using CleanArchitectureDemo.Modules.Catalog.Application.Products.Commands;
using CleanArchitectureDemo.Modules.Catalog.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetProductsQuery());
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id));
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var result = await _sender.Send(new CreateProductCommand(request.Name, request.Description, request.Price, request.StockQuantity, request.CategoryId));
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data) : BadRequest(result.ErrorMessage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateProductDetailsRequest request)
    {
        var result = await _sender.Send(new UpdateProductDetailsCommand(id, request.Name, request.CategoryId));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteProductCommand(id));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPatch("{id}/price")]
    public async Task<IActionResult> SetPrice(int id, [FromBody] decimal newPrice)
    {
        var result = await _sender.Send(new UpdateProductPriceCommand(id, newPrice));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> SetStock(int id, [FromBody] int quantity)
    {
        var result = await _sender.Send(new UpdateProductStockCommand(id, quantity));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var result = await _sender.Send(new ActivateProductCommand(id));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result = await _sender.Send(new DeactivateProductCommand(id));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }

    [HttpPatch("{id}/discontinue")]
    public async Task<IActionResult> Discontinue(int id)
    {
        var result = await _sender.Send(new DiscontinueProductCommand(id));
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateProductDetailsRequest
{
    public string Name { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
}
