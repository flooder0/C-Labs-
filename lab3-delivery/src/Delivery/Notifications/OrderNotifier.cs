using Delivery.Models;
using System.Collections.Generic;

namespace Delivery.Notifications;

public class OrderNotifier
{
    private readonly List<IOrderObserver> _observers = new List<IOrderObserver>();

    public void Subscribe(IOrderObserver observer)
    {
        _observers.Add(observer);
    }

    public void Unsubscribe(IOrderObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyOrderCreated(Order order)
    {
        foreach (var observer in _observers)
        {
            observer.OnOrderCreated(order);
        }
    }

    public void NotifyStateChanged(Order order, string oldState, string newState)
    {
        foreach (var observer in _observers)
        {
            observer.OnOrderStateChanged(order, oldState, newState);
        }
    }
}
