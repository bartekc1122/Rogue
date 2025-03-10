using System.ComponentModel.Design;
using System.Drawing;
using Rogue;

public class Player : IEntity
{

    public Point? Position {get; set;}
    public char Symbol { get => '\u00B6'; }
    public ConsoleColor Color { get => ConsoleColor.Blue; }
    public Statistic Stats;
    public Hands Hands;
    public int Zindex;

    public Inventory Inventory;
    public Player()
    {
        Zindex = 3;
        Stats = new Statistic();
        Inventory = new Inventory();
        Hands = new Hands();
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
    public int Money {get; set;}

    public Statistic(int power = 10, int agility = 15, int health = 20, int luck = 1, int aggro = 3, int wisdom = 10, int money =0)
    {
        Power = power;
        Agility = agility;
        Health = health;
        Luck = luck;
        Aggro = aggro;
        Wisdom = wisdom;
        Money = money;
    }

}