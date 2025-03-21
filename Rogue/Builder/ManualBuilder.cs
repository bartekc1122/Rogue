using System.ComponentModel;
using System.Drawing;
namespace Rogue;

public class ManualBuilder : IBuilder
{
    private bool _hasPickUpInstruction = false;
    private bool _hasMovementInstruction = false;
    private bool _hasInventoryInstruction = false;

    private Manual _manual;

    public void Reset()
    {
        _manual = new Manual();
        _hasInventoryInstruction = false;
        _hasMovementInstruction = false;
        _hasPickUpInstruction = false;
        EmptyDungeon();
        FilledDungeon();
        AddPaths();
    }
#pragma warning disable CS8618 
    public ManualBuilder()
#pragma warning restore CS8618 
    {
        Reset();
    }
    public void EmptyDungeon()
    {
        AddMovementInstructions();
    }
    public void FilledDungeon()
    {
        AddMovementInstructions();
    }
    public void AddPaths()
    {
        AddMovementInstructions();
    }
    public void AddChamberAt(int x, int y, int size)
    {
        AddMovementInstructions();
    }
    public void AddMainChamber(int size)
    {
        AddMovementInstructions();
    }
    public void AddRandomChamber(int size)
    {
        AddMovementInstructions();
    }
    public void ItemsGeneration()
    {
        AddInventoryInstructions();
        AddPickUpInstructions();
    }
    public void WeaponGeneration()
    {
        AddInventoryInstructions();
        AddPickUpInstructions();
    }
    public void DecoratedWeaponGeneration()
    {
        AddInventoryInstructions();
        AddPickUpInstructions();
    }
    public void PotionsGeneration()
    {
        AddPotionInstructions();
        AddInventoryInstructions();
        AddPickUpInstructions();
    }
    public void EnemyGeneration()
    {
        _manual.AddToManual("Normal Attack (z)");
    }
    public Manual getProduct()
    {
        return _manual;
    }
    private void AddMovementInstructions()
    {
        if (!_hasMovementInstruction)
        {
            _manual.AddToManual("Move (w,s,a,d)");
            _hasMovementInstruction = true;
        }
    }
    private void AddPickUpInstructions()
    {
        if (!_hasPickUpInstruction)
        {
            _manual.AddToManual("Pick Up Item (e)");
            _manual.AddToManual("Drop Item (t)");
            _hasPickUpInstruction = true;
        }
    }
    private void AddInventoryInstructions()
    {
        if (!_hasInventoryInstruction)
        {
            _manual.AddToManual("Move item to right hand (1)");
            _manual.AddToManual("Move item to left hand (2)");
            _manual.AddToManual("Select item (j, k)");
            _hasInventoryInstruction = true;
        }
    }
    private void AddPotionInstructions()
    {
        _manual.AddToManual("Drink Potion (U)");
    }
}