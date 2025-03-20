using System.Drawing;

namespace Rogue
{
    public class Mony : IItem
    {
        public char Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public string Name { get; set; }
        public Point? Position { get; set; }
        public bool IsTwoHanded { get; set; }
        public int Value;

        public Mony(string name, char symbol, ConsoleColor color, bool isTwoHanded = false, int value = 1)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            IsTwoHanded = isTwoHanded;
            Value = value;
        }
        public void ApplyOnPickUp(Player player)
        {
            player.Stats.Money += Value;
            player.Inventory.RemoveFromInventory(this);
        }
        public void ApplyOnDePickUp(Player player)
        {
            player.Stats.Money -= Value;
            player.Inventory.AddToInventory(this);
        }
        public override String ToString()
        {
            return Name;
        }
        public void ApplyOnHanded(Player player)
        { }
        public void ApplyOnDeHanded(Player player)
        { }
        public IEntity Clone()
        {
            return new Mony(Name, Symbol, Color, IsTwoHanded, Value);
        }
    }
}