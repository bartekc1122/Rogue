using Rogue;

public class CursedWeapon : WeaponDecorator
{
    public CursedWeapon(IWeapon weapon) : base(weapon)
    { }


    public override ConsoleColor Color => ConsoleColor.DarkRed;

    public override int Damage => _weapon.Damage - 2;


    public override void ApplyOnHanded(Player player)
    {
        _weapon.ApplyOnDeHanded(player);
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
    public override void Accept(ICombatVisitor visitor)
    {
        _weapon.Accept(visitor);
    }

}