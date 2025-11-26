namespace Delivery.Models;

public class CompletedState : IOrderState
{
    public string Name => "Completed";

    public IOrderState? ToPreparing() => null;
    public IOrderState? ToDelivering() => null;
    public IOrderState? Complete() => null;
    public IOrderState? Cancel() => null;
}
