using System.Drawing;
using System.Text.Json.Serialization;
namespace Rogue;

public interface IWeaponComponent
{
    (int damage, int defense) Accept(ICombatVisitor visitor, Player player);
}
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$combatVisitorType")]
[JsonDerivedType(typeof(NormalAttack), "normal_attack_visitor")] 
[JsonDerivedType(typeof(StealthAttack), "stealth_attack_visitor")]  
[JsonDerivedType(typeof(MagicAttack), "magic_attack_visitor")]
public interface ICombatVisitor
{
    (int damage, int defense) DamageHeavyWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageLightWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageMagicWeapon(IWeapon weapon, Player player);
    (int damage, int defense) DamageItem(Player player);
    string MyGetString();
}