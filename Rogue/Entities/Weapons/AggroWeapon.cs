namespace Rogue;
using System.Drawing;
public class AggroWeapon : WeaponDecorator
{
    public override IEntity Clone()
    {
        return new AggroWeapon((IWeapon)_weapon.Clone());
    }

    public AggroWeapon(IWeapon weapon) : base(weapon)
    {
    }
    public override ConsoleColor Color => ConsoleColor.DarkRed;
    public override int Damage => _weapon.Damage + 2;

    public override void ApplyOnHanded(Player player)
    {
        _weapon.ApplyOnHanded(player);
        player.Stats.Aggro += 5;
    }
    public override void ApplyOnDeHanded(Player player)
    {
        _weapon.ApplyOnDeHanded(player);
        player.Stats.Aggro -= 5;
    }

    public override string MyToString()
    {
        return $"{_weapon.MyToString()}(Aggro)";
    }
    public override void Accept(ICombatVisitor visitor)
    {
       _weapon.Accept(visitor); 
    }
}