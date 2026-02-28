using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class ShipperService(INorthwindShipperRepository shipperRepository, ILogger<ShipperService> logger) : IShipperService
{
    public async Task<IEnumerable<dynamic>> GetAllAsync()
    {
        logger.LogTrace(nameof(this.GetAllAsync));
        var shippers = await shipperRepository.GetAllAsync();
        return shippers;
    }
}
