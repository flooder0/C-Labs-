namespace Delivery.Models;

public class CancelledState : IOrderState
{
    public string Name => "Cancelled";

    public IOrderState? ToPreparing() => null;
    public IOrderState? ToDelivering() => null;
    public IOrderState? Complete() => null;
    public IOrderState? Cancel() => null;
}
