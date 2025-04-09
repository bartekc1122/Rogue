using Rogue;
using System.Drawing;
using System.Dynamic;

public class Monster : IMonster
{
    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public int Damage { get; }
    public int Health { get; }
    public int Zindex;

    public Monster(string name, char symbol, ConsoleColor color, int damage, int health)
    {
        Zindex = 3;
        Name = name;
        Symbol = symbol;
        Color = color;
        Damage = damage;
        Health = health;
    }
    public override String ToString()
    {
        return Name;
    }
    public IEntity Clone()
    {
        return new Monster(Name, Symbol, Color, Damage, Health);
    }
}