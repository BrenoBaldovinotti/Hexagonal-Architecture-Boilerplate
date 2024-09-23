using Application.DTOs;

namespace API.DTOs;

public class CreateOrderDto
{
    public required string CustomerName { get; set; }
    public required IEnumerable<OrderItemDto> Items { get; set; }
}
