namespace Domain.Entities;

public class Order(string customerName)
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; } = customerName ?? throw new ArgumentNullException(nameof(customerName));
    public List<OrderItem> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Items.Add(item);
        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(item => item.TotalPrice);
    }
}
