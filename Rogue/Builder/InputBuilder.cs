using System.Runtime.InteropServices;
using Rogue;

public class InputBuilder : IBuilder
{
    private bool EqInput = false;

    private bool PotionsInput = false;

    private IInputHandler _startHandler;
    private IInputHandler _endHandler;
    private GameState _gameState;
    private Logic _logic;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public InputBuilder(GameState gameState, Logic logic)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    {
        _gameState = gameState;
        _logic = logic;
        Reset();
    }

    public void AddChamberAt(int x, int y, int size)
    {
    }

    public void AddMainChamber(int size)
    {
    }

    public void AddPaths()
    {
    }

    public void AddRandomChamber(int size)
    {
    }

    public void DecoratedWeaponGeneration()
    {

        if (!EqInput)
        {
            AddEqInput();
        }
    }

    public void EmptyDungeon()
    {
    }

    public void EnemyGeneration()
    {
        _endHandler = _endHandler.SetNext(new AttackSelectHandler(_gameState));
    }

    public void FilledDungeon()
    {
    }

    public void ItemsGeneration()
    {
        if (!EqInput)
        {
            AddEqInput();
        }
    }

    public void PotionsGeneration()
    {
        if (!EqInput)
        {
            AddEqInput();
        }
        if (!PotionsInput)
        {
            AddPotions();
        }
    }

    public void Reset()
    {
        _startHandler = new WSADHandler(_gameState, _logic);
        _endHandler = _startHandler;
    }
    

    public void WeaponGeneration()
    {
        if (!EqInput)
        {
            AddEqInput();
        }
    }
    public void AddExit()
    {
        _endHandler = _endHandler.SetNext(new ExitHandler(_logic));
    }
    private void AddEqInput()
    {
        var PickUp = new PickUpHandler(_logic);
        var invUp = new MoveCursorUpHandler(_gameState);
        var invDown = new MoveCursorDownHandler(_gameState);
        var throwItem = new ThrowItemHandler(_logic);
        var throwAllItems = new ThrowAllItemHandler(_logic);
        var equipLeft = new EquipLeftHandler(_logic);
        var equipRight = new EquipRightHandler(_logic);
        _endHandler = _endHandler.SetNext(PickUp).SetNext(invUp)
        .SetNext(invDown).SetNext(throwItem).SetNext(throwAllItems)
        .SetNext(equipLeft).SetNext(equipRight);
    }
    private void AddPotions()
    {
        var potionHandle = new DrinkHandler(_logic);
        _endHandler = _endHandler.SetNext(potionHandle);
    }

    public IInputHandler GetProduct()
    {
        var product = _startHandler;
        Reset();
        return product;
    }
}
