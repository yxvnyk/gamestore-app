using Gamestore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetPaindAndCancelledOrders()
    {
        var orders = await orderService.GetPaidAndCancelledOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var orders = await orderService.GetOrderByIdAsync(id);
        return Ok(orders);
    }
}
