using System.Text.Json.Serialization;
using Rogue;

public class CursedWeapon : WeaponDecorator
{
    public CursedWeapon() : base() { }
    [JsonConstructor]
    public CursedWeapon(IWeapon _weapon) : base(_weapon)
    { }

    public override ConsoleColor Color => ConsoleColor.DarkRed;

    public override int Damage => base.Damage - 2;


    public override void ApplyOnHanded(Player player)
    {
        _weapon.ApplyOnHanded(player);
        player.Stats.Luck -= 1;
    }
    public override void ApplyOnDeHanded(Player player)
    {
        _weapon.ApplyOnDeHanded(player);
        player.Stats.Luck += 1;
    }
    public override string MyToString()
    {
        return $"{_weapon.MyToString()}(Cursed)";
    }
    public override IEntity Clone()
    {
        return new CursedWeapon((IWeapon)_weapon.Clone());
    }
    public override (int damage, int defense) Accept(ICombatVisitor visitor, Player player)
    {
        var ret =_weapon.Accept(visitor, player);
        ret.damage -= 2;
        return ret;
    }
}        