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
    public int Health { get; set;}
    public int Zindex;
    public IBehavior Behavior{get; set; }

    public int Defense {get;}
    public Monster(string name, char symbol, ConsoleColor color, int damage, int health, int defense, IBehavior behavior)
    {
        Zindex = 3;
        Name = name;
        Symbol = symbol;
        Color = color;
        Damage = damage;
        Health = health;
        Defense = defense;
        Behavior = behavior;
    }
    public  String MyToString()
    {
        return Name;
    }
    public IEntity Clone()
    {
        return new Monster(Name, Symbol, Color, Damage, Health, Defense, Behavior);
    }
}