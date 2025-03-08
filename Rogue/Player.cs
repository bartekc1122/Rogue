using System.ComponentModel.Design;
using System.Drawing;

public class Player : IEntity
{

    public int XPosition { get; private set; }
    public int YPosition { get; private set; }
    public Point Position { get => new Point(XPosition, YPosition); set { XPosition = value.X; YPosition = value.Y; } }
    public char Symbol { get => '\u00B6'; }
    public ConsoleColor Color { get => ConsoleColor.Blue; }
    public Statistic Stats;


    public Player()
    {
        XPosition = 1;
        YPosition = 1;
        Stats = new Statistic();
    }

}

public class Statistic
{
    public int Power { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }
    public int Luck { get; set; }
    public int Aggro { get; set; }
    public int Wisdom { get; set; }

    public Statistic(int power = 10, int agility = 15, int health = 20, int luck = 1, int aggro = 3, int wisdom = 10)
    {
        Power = power;
        Agility = agility;
        Health = health;
        Luck = luck;
        Aggro = aggro;
        Wisdom = wisdom;
    }

}