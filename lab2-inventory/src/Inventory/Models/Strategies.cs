using System;

namespace Inventory.Models;

// Strategy pattern
public interface ICombinationStrategy
{
    Item? Combine(Item item1, Item item2);
}

public class PotionCombiner : ICombinationStrategy
{
    public Item? Combine(Item item1, Item item2)
    {
        if (item1 is Potion p1 && item2 is Potion p2)
        {
            return new Potion
            {
                Name = "Super " + p1.Name,
                Value = p1.Value + p2.Value,
                HealAmount = p1.HealAmount + p2.HealAmount,
                Weight = p1.Weight + p2.Weight
            };
        }
        return null;
    }
}

public class WeaponRefinement : ICombinationStrategy
{
    public Item? Combine(Item item1, Item item2)
    {
        if (item1 is Weapon w && item2.Type == ItemType.QuestItem) // Assuming quest item is some upgrading stone
        {
            w.Damage += 10;
            w.Value += 50;
            return w;
        }
        return null;
    }
}
