using Application.DTOs;
using MediatR;

namespace Application.Queries.Handlers;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
        }

        var orderDto = new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };

        return orderDto;
    }
}
