namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    public OrderItem(string productName, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrEmpty(productName)) throw new ArgumentNullException(nameof(productName));
        if (unitPrice <= 0) throw new ArgumentException("Unit price must be greater than zero.");
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
