using System.Text.Json.Serialization;
namespace Rogue;
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")] 
[JsonDerivedType(typeof(HeavyWeapon), "heavyweapon")]
[JsonDerivedType(typeof(LightWeapon), "lightweapon")]
[JsonDerivedType(typeof(MagicWeapon), "magicweapon")]
[JsonDerivedType(typeof(AggroWeapon), "aggroweapon")]
[JsonDerivedType(typeof(CursedWeapon), "cursedweapon")]
public interface IWeapon : IItem, IWeaponComponent
{
    int Damage { get; }
}