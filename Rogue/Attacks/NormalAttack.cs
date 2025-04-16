using System.Drawing;
namespace Rogue;

public class NormalAttack : ICombatVisitor
{
    public int DamageHeavyWeapon(IWeapon weapon)
    {
        return weapon.Damage;
    }

    public int DamageLightWeapon(IWeapon weapon)
    {
        return weapon.Damage;
    }

    public int DamageMagicWeapon(IWeapon weapon)
    {
        return 1;
    }
}
public class StealthAttack : ICombatVisitor
{
    public int DamageHeavyWeapon(IWeapon weapon)
    {
        return weapon.Damage / 2;
    }

    public int DamageLightWeapon(IWeapon weapon)
    {
        return weapon.Damage * 2;
    }

    public int DamageMagicWeapon(IWeapon weapon)
    {
        return 1;
    }
}
public class MagicAttack : ICombatVisitor
{
    public int DamageHeavyWeapon(IWeapon weapon)
    {
        return 1;
    }

    public int DamageLightWeapon(IWeapon weapon)
    {
        return 1;
    }

    public int DamageMagicWeapon(IWeapon weapon)
    {
        return weapon.Damage;
    }
}