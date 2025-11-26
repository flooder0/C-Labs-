using Delivery.Models;
using System;
using System.Collections.Generic;

namespace Delivery.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new Dictionary<int, Order>();

    public void Add(Order order)
    {
        if (order != null)
        {
            _orders[order.Id] = order;
        }
    }

    public Order? GetById(int id)
    {
        if (_orders.ContainsKey(id))
        {
            return _orders[id];
        }
        return null;
    }

    public IEnumerable<Order> GetAll()
    {
        return _orders.Values;
    }

    public void Update(Order order)
    {
        if (order != null && _orders.ContainsKey(order.Id))
        {
            _orders[order.Id] = order;
        }
    }

    public void Delete(int id)
    {
        _orders.Remove(id);
    }
}
