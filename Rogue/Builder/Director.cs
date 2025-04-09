using Rogue;

namespace Rogue;

public static class Director
{
    public static void ConstructClassicDungeon(IBuilder builder)
    {
        builder.Reset();
        builder.AddExit(); 
        builder.AddMainChamber(4);
        // builder.AddRandomChamber(2);
        // builder.AddRandomChamber(2);
        // builder.AddRandomChamber(2);
        // builder.AddChamberAt(4, 5, 18);
        // builder.AddChamberAt(4, 36, 3);
        builder.ItemsGeneration();
        builder.WeaponGeneration();
        builder.DecoratedWeaponGeneration();
        builder.PotionsGeneration();
        builder.EnemyGeneration();
    }
}