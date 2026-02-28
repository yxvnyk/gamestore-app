using Gamestore.Application.Services.Interfaces;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Domain.Models.DTO.Payment;
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

    [HttpPost("payment")]
    public async Task<IActionResult> Pay([FromBody] PaymentRequestDto paymentRequestDto)
    {
        var result = await paymentService.ProcessPaymentAsync(paymentRequestDto, StubCustomerId);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        if (result.FileBytes != null)
        {
            return File(result.FileBytes, result.ContentType ?? "application/octet-stream", result.FileName);
        }

        if (result.Data != null)
        {
            return Ok(result.Data);
        }

        // return success if there is no data or file to return
        return Ok();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetOrdersHistory()
    {
        var orderHistory = await orderService.GetOrderHistoryAsync();
        return Ok(orderHistory);
    }
}
