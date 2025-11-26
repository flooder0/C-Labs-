namespace Delivery.Models;

public class PreparingState : IOrderState
{
    public string Name => "Preparing";

    public IOrderState? ToPreparing() => null;

    public IOrderState? ToDelivering()
    {
        return new DeliveringState();
    }

    public IOrderState? Complete() => null;

    public IOrderState? Cancel()
    {
        return new CancelledState();
    }
}
