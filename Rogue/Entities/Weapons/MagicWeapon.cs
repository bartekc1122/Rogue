
using System.Drawing;
namespace Rogue;
public class MagicWeapon : IWeapon
{

    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }
    public int Damage { get; }

    public void Accept(ICombatVisitor visitor)
    {
        visitor.DamageMagicWeapon(this);
    }
    public MagicWeapon(string name = "Stuff", char symbol = 'j', ConsoleColor color = ConsoleColor.DarkMagenta, int damage = 12, bool isTwoHanded = false)
    {
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = isTwoHanded;
        Damage = damage;
    }

    public IEntity Clone()
    {
        return new MagicWeapon(Name, Symbol, Color, Damage, IsTwoHanded);
    }
}