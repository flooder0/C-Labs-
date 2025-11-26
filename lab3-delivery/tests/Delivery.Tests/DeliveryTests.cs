using System;
using System.Linq;
using Delivery.Models;
using Delivery.Notifications;
using Delivery.Pricing;
using Delivery.Repositories;
using Delivery.Services;
using Xunit;

namespace Delivery.Tests;

public class DeliveryTests
{
    [Fact]
    public void Order_ShouldBeCreated_WithCorrectState()
    {
        var repo = new InMemoryOrderRepository();
        var notifier = new OrderNotifier();
        var service = new OrderService(repo, notifier);

        var order = service.CreateOrder(false, false, "");

        Assert.NotNull(order);
        Assert.Equal("Created", order.CurrentState.Name);
    }

    [Fact]
    public void State_ShouldChange_Correctly()
    {
        var repo = new InMemoryOrderRepository();
        var notifier = new OrderNotifier();
        var service = new OrderService(repo, notifier);

        var order = service.CreateOrder(false, false, "");
        
        service.ToPreparing(order.Id);
        Assert.Equal("Preparing", order.CurrentState.Name);

        service.ToDelivering(order.Id);
        Assert.Equal("Delivering", order.CurrentState.Name);

        service.CompleteOrder(order.Id);
        Assert.Equal("Completed", order.CurrentState.Name);
    }

    [Fact]
    public void Pricing_Standard_ShouldSumItems()
    {
        var order = new Order(1);
        order.Items.Add(new OrderItem(new MenuItem { Name = "Pizza", Price = 100 }, 2));
        
        var strategy = new StandardPricingStrategy();
        var total = strategy.CalculateTotal(order);

        Assert.Equal(200, total);
    }

    [Fact]
    public void Pricing_Express_ShouldAddCost()
    {
        var order = new Order(1);
        order.IsExpressDelivery = true;
        order.Items.Add(new OrderItem(new MenuItem { Name = "Pizza", Price = 100 }, 1));

        // Decorator usage in test
        IPricingStrategy strategy = new StandardPricingStrategy();
        strategy = new ExpressDeliveryPricingStrategy(strategy);

        var total = strategy.CalculateTotal(order);

        Assert.Equal(120, total); // 100 * 1.2
    }

    [Fact]
    public void Service_CalculateTotal_ShouldCombineStrategies()
    {
        var repo = new InMemoryOrderRepository();
        var notifier = new OrderNotifier();
        var service = new OrderService(repo, notifier);

        // Express + Preferences
        var order = service.CreateOrder(true, true, ""); 
        service.AddItem(order.Id, new MenuItem { Name = "Burger", Price = 100 }, 1);

        var total = service.CalculateTotal(order.Id);

        // Standard: 100
        // Express: 100 * 1.2 = 120
        // Preferences: 120 + 5 = 125
        Assert.Equal(125, total);
    }

    [Fact]
    public void Observer_ShouldLogChanges()
    {
        var repo = new InMemoryOrderRepository();
        var notifier = new OrderNotifier();
        var observer = new LoggingObserver();
        notifier.Subscribe(observer);
        
        var service = new OrderService(repo, notifier);
        
        var order = service.CreateOrder(false, false, "");
        service.ToPreparing(order.Id);

        Assert.Equal(2, observer.Logs.Count);
        Assert.Contains("created", observer.Logs[0]);
        Assert.Contains("Preparing", observer.Logs[1]);
    }
}

