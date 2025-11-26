namespace Delivery.Models;

public class OrderItem
{
    public MenuItem Item { get; set; }
    public int Quantity { get; set; }
    public decimal ItemTotal => Item.Price * Quantity;

    public OrderItem(MenuItem item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
