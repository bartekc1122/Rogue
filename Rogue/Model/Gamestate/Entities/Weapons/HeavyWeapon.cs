using System.Drawing;
namespace Rogue;
public class HeavyWeapon : IWeapon
{
    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }
    public int Damage { get; }



    public HeavyWeapon(string name = "LongSword", char symbol = 'T', ConsoleColor color = ConsoleColor.Cyan, int damage = 15, bool isTwoHanded = false)
    {
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = isTwoHanded;
        Damage = damage;
    }
    public (int damage, int defense) Accept(ICombatVisitor visitor, Player player)
    {
        return visitor.DamageHeavyWeapon(this, player);
    }


    public IEntity Clone()
    {
        return new HeavyWeapon(Name, Symbol, Color, Damage, IsTwoHanded);
    }
}