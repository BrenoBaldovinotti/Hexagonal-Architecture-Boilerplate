using Application.DTOs;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Handlers;

public class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository, 
    IMemoryCache cache, 
    ILogger<GetOrderByIdQueryHandler> logger) : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Order_{request.OrderId}";

        if (!cache.TryGetValue(cacheKey, out OrderDto? cachedOrder))
        {
            logger.LogInformation("Cache miss for Order {OrderId}, fetching from database.", request.OrderId);
            var order = await orderRepository.GetByIdAsync(request.OrderId);

            if (order != null)
            {
                cachedOrder = new OrderDto
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                         ProductName = item.ProductName,
                         UnitPrice = item.UnitPrice,
                         Quantity = item.Quantity,
                    }).ToList(),
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount
                };

                cache.Set(cacheKey, cachedOrder, _cacheDuration);
                logger.LogInformation("Order {OrderId} cached.", request.OrderId);
            }
        }
        else
        {
            logger.LogInformation("Cache hit for Order {OrderId}.", request.OrderId);
        }

        return cachedOrder;
    }
}
