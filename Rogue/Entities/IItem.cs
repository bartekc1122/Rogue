﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public  interface IItem : IEntity, IWeaponComponent
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
