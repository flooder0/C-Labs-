using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivery.Models;

public class Order
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }
    public string? Address { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    // Flattened properties instead of OrderConditions object
    public bool IsExpressDelivery { get; set; }
    public bool HasPersonalPreferences { get; set; }
    public string? PromoCode { get; set; }
    
    public IOrderState CurrentState { get; set; }

    public Order(int id)
    {
        Id = id;
        CurrentState = new CreatedState(); // Default state
    }

    public decimal CalculateItemsTotal()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.ItemTotal;
        }
        return total;
    }
}
