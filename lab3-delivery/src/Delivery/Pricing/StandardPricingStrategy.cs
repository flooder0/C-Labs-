using Delivery.Models;

namespace Delivery.Pricing;

public class StandardPricingStrategy : IPricingStrategy
{
    public decimal CalculateTotal(Order order)
    {
        return order.CalculateItemsTotal();
    }
}
