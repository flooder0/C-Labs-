using System;

namespace Inventory.Models;

public class WeaponBuilder
{
    private Weapon _weapon;

    public WeaponBuilder()
    {
        _weapon = new Weapon();
    }

    public WeaponBuilder WithName(string name)
    {
        _weapon.Name = name;
        return this;
    }

    public WeaponBuilder WithDamage(int damage)
    {
        _weapon.Damage = damage;
        return this;
    }

    public WeaponBuilder WithAttackSpeed(double speed)
    {
        _weapon.AttackSpeed = speed;
        return this;
    }

    public WeaponBuilder WithWeight(double weight)
    {
        _weapon.Weight = weight;
        return this;
    }

    public WeaponBuilder WithValue(int value)
    {
        _weapon.Value = value;
        return this;
    }

    public Weapon Build()
    {
        // Simple validation
        if (string.IsNullOrEmpty(_weapon.Name))
        {
            _weapon.Name = "Unknown Weapon";
        }
        return _weapon;
    }
}
