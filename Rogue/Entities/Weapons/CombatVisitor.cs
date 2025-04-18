using System.Drawing;
namespace Rogue;

public interface IWeaponComponent
{
    (int damage, int defense) Accept(ICombatVisitor visitor, Player player);
}
public interface ICombatVisitor
{
    (int damage, int defense) DamageHeavyWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageLightWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageMagicWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageItem(Player player);
    string MyGetString();
}