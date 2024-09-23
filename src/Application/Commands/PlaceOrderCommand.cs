using Application.DTOs;
using MediatR;

namespace Application.Commands;

public class PlaceOrderCommand(string customerName, List<OrderItemDto> items) : IRequest<int>
{
    public string CustomerName { get; set; } = customerName;
    public List<OrderItemDto> Items { get; set; } = items;
}
