namespace Rogue;
public interface IWeapon : IItem, IWeaponComponent
{
    int Damage { get; }
}