namespace Inventory.Models;

public abstract class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public double Weight { get; set; }
    public int Value { get; set; }

    public Item()
    {
        Id = Guid.NewGuid();
    }
}

public class Weapon : Item
{
    public int Damage { get; set; }
    public double AttackSpeed { get; set; }

    public Weapon()
    {
        Type = ItemType.Weapon;
    }
}

public class Armor : Item
{
    public int Defense { get; set; }
    
    public Armor()
    {
        Type = ItemType.Armor;
    }
}

public class Potion : Item
{
    public int HealAmount { get; set; }

    public Potion()
    {
        Type = ItemType.Potion;
    }
}
