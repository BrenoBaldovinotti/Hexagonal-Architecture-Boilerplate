namespace Domain.Entities;

public class Order(string customerName)
{
    public int Id { get; private set; }
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
    public string CustomerName { get; private set; } = customerName ?? throw new ArgumentNullException(nameof(customerName));
    public List<OrderItem> Items { get; private set; } = [];
    public decimal TotalAmount { get; private set; }

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
