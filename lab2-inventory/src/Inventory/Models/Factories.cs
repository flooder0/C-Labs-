namespace Inventory.Models;

// Abstract Factory
public interface IItemFactory
{
    Weapon CreateWeapon();
    Armor CreateArmor();
    Potion CreatePotion();
}

public class FantasyFactory : IItemFactory
{
    public Weapon CreateWeapon()
    {
        return new WeaponBuilder()
            .WithName("Iron Sword")
            .WithDamage(15)
            .WithWeight(5)
            .Build();
    }

    public Armor CreateArmor()
    {
        return new Armor { Name = "Plate Mail", Defense = 20, Weight = 15, Value = 100 };
    }

    public Potion CreatePotion()
    {
        return new Potion { Name = "Health Potion", HealAmount = 50, Weight = 0.5, Value = 10 };
    }
}

public class SciFiFactory : IItemFactory
{
    public Weapon CreateWeapon()
    {
         return new WeaponBuilder()
            .WithName("Laser Pistol")
            .WithDamage(30)
            .WithWeight(2)
            .Build();
    }

    public Armor CreateArmor()
    {
        return new Armor { Name = "Nano Suit", Defense = 50, Weight = 1, Value = 500 };
    }

    public Potion CreatePotion()
    {
        return new Potion { Name = "Medkit", HealAmount = 100, Weight = 1, Value = 50 };
    }
}

