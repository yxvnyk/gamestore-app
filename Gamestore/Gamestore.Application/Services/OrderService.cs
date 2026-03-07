using AutoMapper;
using Gamestore.Application.Models;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Order;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class OrderService(IOrderRepository orderRepository,
    INorthwindOrderRepository northwindOrderRepository,
    IOrderItemRepository orderItemRepository, IMapper mapper, ILogger<OrderService> logger) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetOrderHistoryAsync()
    {
        var northwindTask = northwindOrderRepository.GetHistoryAsync();
        var gamestoreTask = orderRepository.GetAllAsync();

        await Task.WhenAll(northwindTask, gamestoreTask);

        var northwindOrders = northwindTask.Result;
        var gamestoreOrders = gamestoreTask.Result;

        var orderHistory = mapper.Map<IEnumerable<OrderDto>>(northwindOrders);
        orderHistory = orderHistory.Concat(mapper.Map<IEnumerable<OrderDto>>(gamestoreOrders));

        return orderHistory;
    }

    public async Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrdersAsync()
    {
        logger.LogInformation(nameof(this.GetPaidAndCancelledOrdersAsync));

        var orders = await orderRepository.GetPaidAndCancelledOrdersAsync();
        return [.. orders.Select(mapper.Map<OrderDto>)];
    }

    public async Task<OrderDto> GetOrderByIdAsync(Identity id)
    {
        logger.LogInformation(nameof(this.GetOrderByIdAsync));

        if (id.IsGuid)
        {
            var order = await orderRepository.GetOrderByIdAsync(id.GuidId!.Value);
            return mapper.Map<OrderDto>(order);
        }

        if (id.IsInt)
        {
            var orderDetails = await northwindOrderRepository.GetOrderByIdAsync(id.IntId!.Value);
            if (orderDetails != null)
            {
                return mapper.Map<OrderDto>(orderDetails);
            }
        }

        return null;
    }

    public async Task<Guid> GetOpenOrderIdAsync(Guid customerId)
    {
        var orderId = await orderRepository.GetOpenOrderIdByCustomerIdAsync(customerId);
        return orderId ?? throw new NotFoundException($"Open order for customer ID {customerId} does not exist.");
    }

    public async Task<double> CalculataOrderTotalAsync(Guid orderId)
    {
        logger.LogInformation(nameof(this.CalculataOrderTotalAsync));
        var items = await orderItemRepository.GetByOrderIdAsync(orderId);
        return items.Sum(x => x.Price * x.Quantity);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        logger.LogInformation(nameof(this.DeleteByIdAsync));
        return await orderRepository.DeleteByIdAsync(id);
    }

    public async Task UpdateOrderStatusAsync(Order order, OrderStatus status)
    {
        order.Status = status;
        await orderRepository.UpdateAsync(order);
    }

    public async Task UpdateAsync(Order entity)
    {
        await orderRepository.UpdateAsync(entity);
    }
}
