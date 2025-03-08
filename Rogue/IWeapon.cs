using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    interface IWeapon
    {
        public Char Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public Point? Position {  get; set; }
        public String Name { get; }
    }
}
