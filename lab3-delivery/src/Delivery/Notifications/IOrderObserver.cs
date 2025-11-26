using Delivery.Models;

namespace Delivery.Notifications;

public interface IOrderObserver
{
    void OnOrderCreated(Order order);
    void OnOrderStateChanged(Order order, string oldState, string newState);
}
