using System.Drawing;
namespace Rogue;

public class NormalAttack : ICombatVisitor
{
    public (int damage, int defense) DamageHeavyWeapon(IWeapon weapon, Player player)
    {
        return (weapon.Damage, player.Stats.Power + player.Stats.Luck);
    }

    public (int damage, int defense) DamageLightWeapon(IWeapon weapon, Player player)
    {
        return (weapon.Damage, player.Stats.Agility + player.Stats.Luck);
    }

    public (int damage, int defense) DamageMagicWeapon(IWeapon weapon, Player player)
    {
        return (1, player.Stats.Agility + player.Stats.Luck);
    }
    public (int damage, int defense) DamageItem(Player player)
    {
        return (0, player.Stats.Agility);
    }
    public string MyGetString()
    {
        return "Normal Attack";
    }
}
public class StealthAttack : ICombatVisitor
{
    public (int damage, int defense) DamageHeavyWeapon(IWeapon weapon, Player player)
    {
        return (weapon.Damage / 2, player.Stats.Power);
    }

    public (int damage, int defense) DamageLightWeapon(IWeapon weapon, Player player)
    {
        return (weapon.Damage * 2, player.Stats.Agility);
    }

    public (int damage, int defense) DamageMagicWeapon(IWeapon weapon, Player player)
    {
        return (1, 0);
    }
    public (int damage, int defense) DamageItem(Player player)
    {
        return (0, 0);
    }
    public string MyGetString()
    {
        return "Stealth Attack";
    }
}
public class MagicAttack : ICombatVisitor
{
    public (int damage, int defense) DamageHeavyWeapon(IWeapon weapon, Player player)
    {
        return (1, player.Stats.Luck);
    }

    public (int damage, int defense) DamageLightWeapon(IWeapon weapon, Player player)
    {
        return (1, player.Stats.Luck);
    }

    public (int damage, int defense) DamageMagicWeapon(IWeapon weapon, Player player)
    {
        return (weapon.Damage, player.Stats.Wisdom * 2);
    }
    public (int damage, int defense) DamageItem(Player player)
    {
        return (0, player.Stats.Luck);
    }
    public string MyGetString()
    {
        return "Magic Attack";
    }
}