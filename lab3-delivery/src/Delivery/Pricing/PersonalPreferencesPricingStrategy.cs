using Delivery.Models;

namespace Delivery.Pricing;

public class PersonalPreferencesPricingStrategy : IPricingStrategy
{
    private readonly IPricingStrategy _baseStrategy;

    public PersonalPreferencesPricingStrategy(IPricingStrategy baseStrategy)
    {
        _baseStrategy = baseStrategy;
    }

    public decimal CalculateTotal(Order order)
    {
        var total = _baseStrategy.CalculateTotal(order);
        if (order.HasPersonalPreferences)
        {
            return total + 5.00m; // Flat fee
        }
        return total;
    }
}
