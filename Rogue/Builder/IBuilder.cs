using System.IO.Compression;

namespace Rogue;

public interface IBuilder
{
    public void Reset();
    public void EmptyDungeon();
    public void FilledDungeon();
    public void AddChamberAt(int x, int y, int size);
    public void AddMainChamber(int size);
    public void AddRandomChamber(int size);
    public void ItemsGeneration();
    public void WeaponGeneration();
    public void DecoratedWeaponGeneration();
    public void PotionsGeneration();
    public void EnemyGeneration();
    public void AddPaths();
    public void AddExit();
}