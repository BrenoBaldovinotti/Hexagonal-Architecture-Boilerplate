namespace Application.DTOs;

public class OrderItemDto
{
    public required string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
