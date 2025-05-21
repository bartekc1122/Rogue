namespace Rogue;
using System.Drawing;
using System.Text.Json.Serialization;

public class AggroWeapon : WeaponDecorator
{
    public AggroWeapon() : base() { }
    [JsonConstructor]
    public AggroWeapon(IWeapon _weapon) : base(_weapon)
    {
    }
    public override IEntity Clone()
    {
        return new AggroWeapon((IWeapon)_weapon.Clone());
    }
    public override ConsoleColor Color => ConsoleColor.DarkRed;
    public override int Damage => base.Damage + 2;

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
    public override (int damage, int defense) Accept(ICombatVisitor visitor, Player player)
    {
        return _weapon.Accept(visitor, player);
    }
}