using Delivery.Models;

namespace Delivery.Pricing;

public class PromoCodePricingStrategy : IPricingStrategy
{
    private readonly IPricingStrategy _baseStrategy;

    public PromoCodePricingStrategy(IPricingStrategy baseStrategy)
    {
        _baseStrategy = baseStrategy;
    }

    public decimal CalculateTotal(Order order)
    {
        var total = _baseStrategy.CalculateTotal(order);
        if (!string.IsNullOrEmpty(order.PromoCode) && order.PromoCode == "SALE")
        {
            return total * 0.9m; // 10% discount
        }
        return total;
    }
}
