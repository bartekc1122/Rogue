using System.Drawing;
using System.Text.Json.Serialization;
using Rogue;
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Player), "player")]
[JsonDerivedType(typeof(Item), "item_generic")]
[JsonDerivedType(typeof(Potion), "potion")]
[JsonDerivedType(typeof(Monster), "monster")]
[JsonDerivedType(typeof(HeavyWeapon), "heavyweapon")]
[JsonDerivedType(typeof(LightWeapon), "lightweapon")]
[JsonDerivedType(typeof(MagicWeapon), "magicweapon")]
[JsonDerivedType(typeof(AggroWeapon), "aggroweapon")]
[JsonDerivedType(typeof(CursedWeapon), "cursedweapon")]
[JsonDerivedType(typeof(Mony), "mony")]
public interface IEntity
{
    Point? Position { get; set; }
    char Symbol { get; }
    ConsoleColor Color { get; }
    [JsonIgnore]
    int ZIndex { get => 1; }
    public IEntity Clone();
}