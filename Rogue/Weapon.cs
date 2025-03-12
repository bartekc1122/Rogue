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
        public char Symbol { get; }
        public ConsoleColor Color { get; }
        public string Name { get; }
        public Point? Position { get; set; }
        public bool IsTwoHanded { get; }
        public int Damage { get; }

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
        { }
        public void ApplyOnHanded(Player player)
        { }
        public void ApplyOnDeHanded(Player player)
        { }
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
        }

        public void ApplyOnDePickUp(Player player)
        {
            _weapon.ApplyOnDePickUp(player);
        }

        public void ApplyOnHanded(Player player)
        {
            _weapon.ApplyOnDeHanded(player);
            player.Stats.Luck -= 1;
        }
        public void ApplyOnDeHanded(Player player)
        {
            _weapon.ApplyOnDeHanded(player);
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
        }

        public void ApplyOnDePickUp(Player player)
        {
            _weapon.ApplyOnDePickUp(player);
        }
        public void ApplyOnHanded(Player player)
        {
            _weapon.ApplyOnHanded(player);
            player.Stats.Aggro += 5;
        }
        public void ApplyOnDeHanded(Player player)
        {
            _weapon.ApplyOnDeHanded(player);
            player.Stats.Aggro -= 5;
        }

        public override string ToString()
        {
            return $"{_weapon.ToString()}(Aggro)";
        }
    }
}

