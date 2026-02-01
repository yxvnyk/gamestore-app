using Gamestore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService, IOrderItemService cartService, IPaymentService paymentService) : Controller
{
    private static readonly Guid StubCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

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

    [HttpDelete("cart/{key}")]
    public async Task<IActionResult> DeleteGameFromCart(string key)
    {
        await cartService.DeleteAsync(key, StubCustomerId);
        return NoContent();
    }

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetOrderDetailsById(Guid id)
    {
        var orderDetails = await cartService.GetOrderItemsByOrderIdAsync(id);
        return Ok(orderDetails);
    }

    [HttpGet("cart")]
    public async Task<IActionResult> GetCart()
    {
        var orderDetails = await cartService.GetCartByCustomerId(StubCustomerId);
        return Ok(orderDetails);
    }

    [HttpGet("payment-methods")]
    public IActionResult GetPaymentMethods()
    {
        return Ok(paymentService.GetPaymentMethods());
    }
}
