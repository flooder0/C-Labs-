namespace Delivery.Models;

public class CreatedState : IOrderState
{
    public string Name => "Created";

    public IOrderState? ToPreparing()
    {
        return new PreparingState();
    }

    public IOrderState? ToDelivering()
    {
        return null; 
    }

    public IOrderState? Complete()
    {
        return null;
    }

    public IOrderState? Cancel()
    {
        return new CancelledState();
    }
}
