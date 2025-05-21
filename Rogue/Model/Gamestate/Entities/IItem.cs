using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Rogue
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")] 
    [JsonDerivedType(typeof(Item), "item_generic")]
    [JsonDerivedType(typeof(Potion), "potion")]
    [JsonDerivedType(typeof(Mony), "mony")]
    [JsonDerivedType(typeof(HeavyWeapon), "heavyweapon")]
    [JsonDerivedType(typeof(LightWeapon), "lightweapon")]
    [JsonDerivedType(typeof(MagicWeapon), "magicweapon")]
    [JsonDerivedType(typeof(AggroWeapon), "aggroweapon")]
    [JsonDerivedType(typeof(CursedWeapon), "cursedweapon")]
    public interface IItem : IEntity, IWeaponComponent
    {
        public string Name { get; }
        public string MyToString() { return Name ?? "Not set!"; }
        public bool IsTwoHanded { get; }

        public void ApplyOnPickUp(Player player) { }
        public void ApplyOnDePickUp(Player player) { }
        public void ApplyOnHanded(Player player) { }
        public void ApplyOnDeHanded(Player player) { }
        public bool Drink(Player player) => false;
        public new (int damage, int defense) Accept(ICombatVisitor visitor, Player player)
        {
            return visitor.DamageItem(player);
        }
    }

}
