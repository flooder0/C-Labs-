using Delivery.Models;
using System.Collections.Generic;

namespace Delivery.Notifications;

public class LoggingObserver : IOrderObserver
{
    public List<string> Logs { get; } = new List<string>();

    public void OnOrderCreated(Order order)
    {
        Logs.Add($"Order {order.Id} created");
    }

    public void OnOrderStateChanged(Order order, string oldState, string newState)
    {
        Logs.Add($"Order {order.Id} state changed: {oldState} -> {newState}");
    }
}
