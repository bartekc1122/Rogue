using Rogue;
using System.Drawing;


public class Potion : IPotion 
{
    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }

    public Potion(string name, char symbol, ConsoleColor color)
    {
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = false;
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
    public IEntity Clone()
    {
        return new Item(Name, Symbol, Color, IsTwoHanded);
    }
}