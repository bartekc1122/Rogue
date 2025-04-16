using System.Drawing;
namespace Rogue;
public class LightWeapon : IWeapon
{

    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }
    public int Damage { get; }
    public void Accept(ICombatVisitor visitor)
    {
        visitor.DamageLightWeapon(this);
    }


    public LightWeapon(string name = "ShortSword", char symbol = 't', ConsoleColor color = ConsoleColor.DarkGray, int damage = 10, bool isTwoHanded = false)
    {
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = isTwoHanded;
        Damage = damage;
    }

    public IEntity Clone()
    {
        return new LightWeapon(Name, Symbol, Color, Damage, IsTwoHanded);
    }
}