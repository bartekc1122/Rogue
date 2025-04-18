namespace Rogue;
public interface IMonster : IEntity
{
    public String Name { get; }
    int Damage { get; }
    int Health { get; set; }
    int Defense { get; }

}