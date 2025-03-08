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

        public Weapon(string name, char symbol, ConsoleColor color, Point? position)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            Position = position;
        }
    }
}

