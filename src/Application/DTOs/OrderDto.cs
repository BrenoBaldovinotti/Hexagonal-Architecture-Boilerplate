namespace Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public required string CustomerName { get; set; }
    public required IList<OrderItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
