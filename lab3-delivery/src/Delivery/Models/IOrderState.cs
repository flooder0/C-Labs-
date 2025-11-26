namespace Delivery.Models;

public interface IOrderState
{
    string Name { get; }
    IOrderState? ToPreparing();
    IOrderState? ToDelivering();
    IOrderState? Complete();
    IOrderState? Cancel();
}
