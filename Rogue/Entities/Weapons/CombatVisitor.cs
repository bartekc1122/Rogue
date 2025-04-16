using System.Drawing;
namespace Rogue;

public interface IWeaponComponent
{
    void Accept(ICombatVisitor visitor)
    {
        visitor.DamageItem();
    }
}
public interface ICombatVisitor
{
    int DamageHeavyWeapon(IWeapon weapon);
    int DamageLightWeapon(IWeapon weapon);
    int DamageMagicWeapon(IWeapon weapon);
    int DamageItem()
    {
        return 0;
    }
}