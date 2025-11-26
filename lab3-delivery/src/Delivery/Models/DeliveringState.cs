namespace Delivery.Models;

public class DeliveringState : IOrderState
{
    public string Name => "Delivering";

    public IOrderState? ToPreparing() => null;
    public IOrderState? ToDelivering() => null;

    public IOrderState? Complete()
    {
        return new CompletedState();
    }

    public IOrderState? Cancel()
    {
        return new CancelledState();
    }
}
