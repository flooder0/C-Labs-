using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Models;

public class Inventory
{
    private List<InventoryItem> items = new List<InventoryItem>();
    public Guid PlayerId { get; set; }

    public Inventory(Guid playerId)
    {
        PlayerId = playerId;
    }

    public void AddItem(Item item)
    {
        if (item == null) return;
        items.Add(new InventoryItem(item));
    }

    public void RemoveItem(Guid itemId)
    {
        var itemToRemove = items.FirstOrDefault(i => i.Item.Id == itemId);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
        }
    }

    public InventoryItem? GetItem(Guid itemId)
    {
        return items.FirstOrDefault(i => i.Item.Id == itemId);
    }

    public List<InventoryItem> GetAllItems()
    {
        return items;
    }

    public void CombineItems(Guid id1, Guid id2, ICombinationStrategy strategy)
    {
        var item1 = GetItem(id1);
        var item2 = GetItem(id2);

        if (item1 != null && item2 != null)
        {
            var newItem = strategy.Combine(item1.Item, item2.Item);
            if (newItem != null)
            {
                items.Remove(item1);
                items.Remove(item2);
                AddItem(newItem);
            }
        }
    }
}
