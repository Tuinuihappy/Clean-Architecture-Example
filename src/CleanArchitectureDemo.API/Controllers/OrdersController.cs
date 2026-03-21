using CleanArchitectureDemo.Modules.Ordering.Application.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var result = await _sender.Send(new PlaceOrderCommand(request.ProductId, request.ProductName, request.UnitPrice, request.Quantity));
        return result.IsSuccess 
            ? Ok(new { OrderId = result.Data, Message = "Order placed successfully!" }) 
            : BadRequest(result.ErrorMessage);
    }
}

public class PlaceOrderRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
