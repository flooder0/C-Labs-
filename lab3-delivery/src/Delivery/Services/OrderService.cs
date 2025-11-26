using System;
using System.Collections.Generic;
using System.Linq;
using Delivery.Models;
using Delivery.Notifications;
using Delivery.Pricing;
using Delivery.Repositories;

namespace Delivery.Services;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly OrderNotifier _notifier;
    private int _nextOrderId = 1;

    public OrderService(IOrderRepository repository, OrderNotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public Order CreateOrder(bool isExpress, bool hasPreferences, string promoCode)
    {
        var order = new Order(_nextOrderId++);
        order.IsExpressDelivery = isExpress;
        order.HasPersonalPreferences = hasPreferences;
        order.PromoCode = promoCode;
        
        _repository.Add(order);
        _notifier.NotifyOrderCreated(order);
        return order;
    }

    public void AddItem(int orderId, MenuItem item, int quantity)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            order.Items.Add(new OrderItem(item, quantity));
            _repository.Update(order);
        }
    }

    public void ToPreparing(int orderId)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            var oldState = order.CurrentState.Name;
            var newState = order.CurrentState.ToPreparing();
            if (newState != null)
            {
                order.CurrentState = newState;
                _repository.Update(order);
                _notifier.NotifyStateChanged(order, oldState, newState.Name);
            }
            else
            {
                Console.WriteLine("Cannot move to preparing state.");
            }
        }
    }

    public void ToDelivering(int orderId)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            var oldState = order.CurrentState.Name;
            var newState = order.CurrentState.ToDelivering();
            if (newState != null)
            {
                order.CurrentState = newState;
                _repository.Update(order);
                _notifier.NotifyStateChanged(order, oldState, newState.Name);
            }
        }
    }

    public void CompleteOrder(int orderId)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            var oldState = order.CurrentState.Name;
            var newState = order.CurrentState.Complete();
            if (newState != null)
            {
                order.CurrentState = newState;
                _repository.Update(order);
                _notifier.NotifyStateChanged(order, oldState, newState.Name);
            }
        }
    }

    public decimal CalculateTotal(int orderId)
    {
        var order = _repository.GetById(orderId);
        if (order == null) return 0;

        // Manual composition of strategy (Decorator)
        IPricingStrategy strategy = new StandardPricingStrategy();

        if (order.IsExpressDelivery)
        {
            strategy = new ExpressDeliveryPricingStrategy(strategy);
        }
        
        if (order.HasPersonalPreferences)
        {
            strategy = new PersonalPreferencesPricingStrategy(strategy);
        }

        if (!string.IsNullOrEmpty(order.PromoCode))
        {
            strategy = new PromoCodePricingStrategy(strategy);
        }

        return strategy.CalculateTotal(order);
    }
}
