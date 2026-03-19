using CleanArchitectureDemo.Modules.Ordering.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var result = await _orderService.PlaceOrderAsync(request.ProductId, request.ProductName, request.UnitPrice, request.Quantity);
        
        if (result.IsSuccess)
        {
            return Ok(new { OrderId = result.Data, Message = "Order placed successfully!" });
        }
        
        return BadRequest(result.ErrorMessage);
    }
}

public class PlaceOrderRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
