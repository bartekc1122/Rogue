using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public class Weapon : IWeapon
    {
        public char Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public string Name { get; set; }
        public Point? Position { get; set; }
        public bool IsTwoHanded { get; set; }

        public int Damage { get; set; }

        public Weapon(string name, char symbol, ConsoleColor color, int damage, bool isTwoHanded = false)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            IsTwoHanded = isTwoHanded;
            Damage = damage;
        }
        public void ApplyOnPickUp(Player player)
        { }
        public void ApplyOnDePickUp(Player player)
        {
        }
        public override String ToString()
        {
            return Name;
        }
    }
    public class CursedWeapon : IWeapon
    {
        private readonly IWeapon _weapon;

        public CursedWeapon(IWeapon weapon)
        {
            _weapon = weapon;
        }

        public IWeapon BaseWeapon => _weapon;

        public char Symbol => _weapon.Symbol;
        public ConsoleColor Color => ConsoleColor.DarkRed;
        public string Name => _weapon.Name;
        public Point? Position { get => _weapon.Position; set => _weapon.Position = value; }
        public bool IsTwoHanded => _weapon.IsTwoHanded;

        public int Damage => _weapon.Damage - 2;

        public void ApplyOnPickUp(Player player)
        {
            _weapon.ApplyOnPickUp(player);
            player.Stats.Luck -= 1;
        }

        public void ApplyOnDePickUp(Player player)
        {
            _weapon.ApplyOnDePickUp(player);
            player.Stats.Luck += 1;
        }

        public override string ToString()
        {
            return $"{_weapon.ToString()}(Cursed)";
        }
    }
    public class AggroWeapon : IWeapon
    {
        private readonly IWeapon _weapon;

        public AggroWeapon(IWeapon weapon)
        {
            _weapon = weapon;
        }

        public IWeapon BaseWeapon => _weapon;

        public char Symbol => _weapon.Symbol;
        public ConsoleColor Color => ConsoleColor.DarkRed;
        public string Name => _weapon.Name;
        public Point? Position { get => _weapon.Position; set => _weapon.Position = value; }
        public bool IsTwoHanded => _weapon.IsTwoHanded;

        public int Damage => _weapon.Damage + 2;

        public void ApplyOnPickUp(Player player)
        {
            _weapon.ApplyOnPickUp(player);
            player.Stats.Aggro += 5;
        }

        public void ApplyOnDePickUp(Player player)
        {
            _weapon.ApplyOnDePickUp(player);
            player.Stats.Aggro -= 5;
        }

        public override string ToString()
        {
            return $"{_weapon.ToString()}(Aggro)";
        }
    }
}

