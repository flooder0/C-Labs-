using Delivery.Models;

namespace Delivery.Pricing;

public interface IPricingStrategy
{
    decimal CalculateTotal(Order order);
}
