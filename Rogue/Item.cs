using System.Drawing;
using Rogue;


public class Item : IItem
{
    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }

    public Item(string name, char symbol, ConsoleColor color, bool isTwoHanded = false)
    {
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = isTwoHanded;
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