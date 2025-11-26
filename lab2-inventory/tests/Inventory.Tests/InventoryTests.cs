using System;
using System.Linq;
using Inventory.Models;
using Xunit;

namespace Inventory.Tests;

public class InventoryTests
{
    [Fact]
    public void AddItem_ShouldAddToList()
    {
        var inventory = new Models.Inventory(Guid.NewGuid());
        var weapon = new Weapon { Name = "Sword", Damage = 10 };

        inventory.AddItem(weapon);

        var items = inventory.GetAllItems();
        Assert.Single(items);
        Assert.Equal("Sword", items[0].Item.Name);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveFromList()
    {
        var inventory = new Models.Inventory(Guid.NewGuid());
        var weapon = new Weapon { Name = "Sword" };
        inventory.AddItem(weapon);

        inventory.RemoveItem(weapon.Id);

        Assert.Empty(inventory.GetAllItems());
    }

    [Fact]
    public void Factory_ShouldCreateCorrectType()
    {
        IItemFactory factory = new FantasyFactory();
        
        var weapon = factory.CreateWeapon();
        var armor = factory.CreateArmor();

        Assert.IsType<Weapon>(weapon);
        Assert.IsType<Armor>(armor);
        Assert.Equal("Iron Sword", weapon.Name);
    }

    [Fact]
    public void Strategy_ShouldCombinePotions()
    {
        var inventory = new Models.Inventory(Guid.NewGuid());
        var p1 = new Potion { Name = "Small HP", HealAmount = 10, Value = 5, Weight = 1 };
        var p2 = new Potion { Name = "Small HP", HealAmount = 10, Value = 5, Weight = 1 };

        inventory.AddItem(p1);
        inventory.AddItem(p2);

        ICombinationStrategy strategy = new PotionCombiner();
        inventory.CombineItems(p1.Id, p2.Id, strategy);

        var items = inventory.GetAllItems();
        Assert.Single(items);
        Assert.Equal("Super Small HP", items[0].Item.Name);
        Assert.Equal(20, ((Potion)items[0].Item).HealAmount);
    }

    [Fact]
    public void State_ShouldChangeToEquipped()
    {
        var weapon = new Weapon { Name = "Axe" };
        var invItem = new InventoryItem(weapon);

        Assert.IsType<InInventoryState>(invItem.State);

        invItem.Equip();

        Assert.IsType<EquippedState>(invItem.State);
    }

    [Fact]
    public void Builder_ShouldBuildWeapon()
    {
        var builder = new WeaponBuilder();
        var weapon = builder
            .WithName("Excalibur")
            .WithDamage(100)
            .WithWeight(10)
            .Build();

        Assert.Equal("Excalibur", weapon.Name);
        Assert.Equal(100, weapon.Damage);
        Assert.Equal(10, weapon.Weight);
    }
}

