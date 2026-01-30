using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Order;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrdersAsync()
    {
        logger.LogInformation(nameof(this.GetPaidAndCancelledOrdersAsync));

        var orders = await orderRepository.GetPaidAndCancelledOrdersAsync();
        return [.. orders.Select(mapper.Map<OrderDto>)];
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid id)
    {
        logger.LogInformation(nameof(this.GetOrderByIdAsync));

        var order = await orderRepository.GetOrderByIdAsync(id);
        return order is not null ? mapper.Map<OrderDto>(order) : throw new NotFoundException($"Order with ID {id} does not exist.");
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        logger.LogInformation(nameof(this.DeleteByIdAsync));
        return await orderRepository.DeleteByIdAsync(id);
    }

    public async Task UpdateGenreAsync(Order entity)
    {
        await orderRepository.UpdateGenreAsync(entity);
    }
}
