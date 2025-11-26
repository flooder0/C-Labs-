using Delivery.Models;

namespace Delivery.Pricing;

public class ExpressDeliveryPricingStrategy : IPricingStrategy
{
    private readonly IPricingStrategy _baseStrategy;

    public ExpressDeliveryPricingStrategy(IPricingStrategy baseStrategy)
    {
        _baseStrategy = baseStrategy;
    }

    public decimal CalculateTotal(Order order)
    {
        var total = _baseStrategy.CalculateTotal(order);
        if (order.IsExpressDelivery)
        {
            return total * 1.20m; // 20% extra
        }
        return total;
    }
}
