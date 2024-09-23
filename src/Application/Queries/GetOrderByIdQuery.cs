using Application.DTOs;
using MediatR;

namespace Application.Queries;

public class GetOrderByIdQuery(int orderId) : IRequest<OrderDto>
{
    public int OrderId { get; set; } = orderId;
}
