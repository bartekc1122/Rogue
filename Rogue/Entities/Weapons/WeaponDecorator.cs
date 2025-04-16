using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
namespace Rogue;
public abstract class WeaponDecorator : IWeapon
{
    protected readonly IWeapon _weapon;

    public WeaponDecorator(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public IWeapon BaseWeapon => _weapon;
    public abstract void Accept(ICombatVisitor visitor);

    public virtual char Symbol => _weapon.Symbol;
    public virtual ConsoleColor Color => _weapon.Color;
    public virtual string Name => _weapon.Name;
    public Point? Position { get => _weapon.Position; set => _weapon.Position = value; }
    public virtual bool IsTwoHanded => _weapon.IsTwoHanded;

    public virtual int Damage => _weapon.Damage;

    public virtual void ApplyOnPickUp(Player player)
    {
        _weapon.ApplyOnPickUp(player);
    }

    public virtual void ApplyOnDePickUp(Player player)
    {
        _weapon.ApplyOnDePickUp(player);
    }

    public virtual void ApplyOnHanded(Player player)
    {
        _weapon.ApplyOnDeHanded(player);
    }
    public virtual void ApplyOnDeHanded(Player player)
    {
        _weapon.ApplyOnDeHanded(player);
    }
    public virtual string MyToString()
    {
        return $"{_weapon.MyToString()}";
    }
    public abstract IEntity Clone();

}