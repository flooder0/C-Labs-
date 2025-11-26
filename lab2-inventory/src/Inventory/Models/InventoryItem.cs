using System;

namespace Inventory.Models;

// State pattern interface
public interface IItemState
{
    void Use(InventoryItem item);
    void Equip(InventoryItem item);
}

// Concrete states
public class InInventoryState : IItemState
{
    public void Use(InventoryItem item)
    {
        Console.WriteLine($"Using {item.Item.Name} from inventory...");
        // Logic for using item
    }

    public void Equip(InventoryItem item)
    {
        Console.WriteLine($"Equipping {item.Item.Name}...");
        item.SetState(new EquippedState());
    }
}

public class EquippedState : IItemState
{
    public void Use(InventoryItem item)
    {
        Console.WriteLine($"Cannot use {item.Item.Name} while equipped!");
    }

    public void Equip(InventoryItem item)
    {
        Console.WriteLine($"{item.Item.Name} is already equipped.");
    }
}

public class InventoryItem
{
    public Item Item { get; set; }
    public IItemState State { get; private set; }

    public InventoryItem(Item item)
    {
        Item = item;
        State = new InInventoryState(); // Default state
    }

    public void SetState(IItemState newState)
    {
        State = newState;
    }

    public void Use()
    {
        State.Use(this);
    }

    public void Equip()
    {
        State.Equip(this);
    }
}

