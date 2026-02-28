using Gamestore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ShippersController(IShipperService shipperService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAllShippersAsync()
    {
        var shippers = await shipperService.GetAllAsync();
        return Ok(shippers);
    }
}
