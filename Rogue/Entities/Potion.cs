using Rogue;
using System.Drawing;


public class Potion : IItem
{
    public char Symbol { get; }
    public ConsoleColor Color { get; }
    public string Name { get; }
    public Point? Position { get; set; }
    public bool IsTwoHanded { get; }
    public AEffect Effect { get; private set; }

    public Potion(string name, char symbol, ConsoleColor color, AEffect effect)
    {
        Effect = effect;
        Name = name;
        Symbol = symbol;
        Color = color;
        IsTwoHanded = false;
    }
    virtual public void ApplyOnPickUp(Player player)
    { }
    virtual public void ApplyOnDePickUp(Player player)
    { }
    public void ApplyOnHanded(Player player)
    { }
    public void ApplyOnDeHanded(Player player)
    { }
    public bool Drink(Player player)
    {
        var effect = Effect;
        player.effects.Add(effect);
        return true;
    }
    public String MyToString()
    {
        return Name;
    }
    public IEntity Clone()
    {
        return new Potion(Name, Symbol, Color, Effect.Clone(Effect));
    }
    public (int damage, int defense) Accept(ICombatVisitor visitor, Player player)
    {
        return visitor.DamageItem(player);
    }
}
