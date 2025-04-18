using System.Drawing;
using Rogue;


public interface IInputHandler
{
    IInputHandler SetNext(IInputHandler handler);
    object? Handle(object request);
}

abstract class AbstractInputHandler : IInputHandler
{
    public IInputHandler? _nextHandler { private set; get; }
    public IInputHandler SetNext(IInputHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }
    public virtual object? Handle(object request)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(request);
        }
        else
        {
            return -1;
        }
    }
}

class WSADHandler : AbstractInputHandler
{
    private GameState _gameState;
    private Logic _logic;
    public WSADHandler(GameState gameState, Logic logic)
    {
        _gameState = gameState;
        _logic = logic;
    }

    public override object? Handle(object request)
    {
        Point newPosition = _gameState.EntityManager.GetEntityPosition(_gameState.Player);
        switch (request as ConsoleKey?)
        {
            case ConsoleKey.W:
                newPosition.Y--;
                var enemy = _gameState.EntityManager.MoveEntity(_gameState.Player, newPosition);
                if (enemy != null)
                {
                    return _logic.Fight(_gameState.Player, enemy);
                }
                return "Moved down";
            case ConsoleKey.S:
                newPosition.Y++;
                enemy = _gameState.EntityManager.MoveEntity(_gameState.Player, newPosition);
                if (enemy != null)
                {
                    return _logic.Fight(_gameState.Player, enemy);
                }
                return "Moved up";
            case ConsoleKey.A:
                newPosition.X--;
                enemy = _gameState.EntityManager.MoveEntity(_gameState.Player, newPosition);
                if (enemy != null)
                {
                    return _logic.Fight(_gameState.Player, enemy);
                }
                return "Moved left";
            case ConsoleKey.D:
                newPosition.X++;
                enemy = _gameState.EntityManager.MoveEntity(_gameState.Player, newPosition);
                if (enemy != null)
                {
                    return _logic.Fight(_gameState.Player, enemy);
                }
                return "Moved right";
            default:
                return base.Handle(request);
        }
    }
}

class PickUpHandler : AbstractInputHandler
{
    private Logic _logic;
    public PickUpHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.E)
        {
            var success = _logic.TryPickUpItem();
            return success ? "Item picked up" : "Nothing to pick up";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class MoveCursorUpHandler : AbstractInputHandler
{
    private GameState _gameState;
    public MoveCursorUpHandler(GameState gameState)
    {
        _gameState = gameState;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.J)
        {
            _gameState.Player.Inventory.MoveCursor(1);
            return "Cursor moved up";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class MoveCursorDownHandler : AbstractInputHandler
{
    private GameState _gameState;
    public MoveCursorDownHandler(GameState gameState)
    {
        _gameState = gameState;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.K)
        {
            _gameState.Player.Inventory.MoveCursor(-1);
            return "Cursor moved down";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class ThrowItemHandler : AbstractInputHandler
{
    private Logic _logic;
    public ThrowItemHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.T)
        {
            _logic.TryThrowItem();
            return "Item thrown";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class ThrowAllItemHandler : AbstractInputHandler
{
    private Logic _logic;
    public ThrowAllItemHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.O)
        {
            _logic.TryThrowAllItems();
            return "All items thrown";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class EquipRightHandler : AbstractInputHandler
{
    private Logic _logic;
    public EquipRightHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.D1)
        {
            var eq = _logic.EquipRight();
            if (eq)
            {
                return "Item equipped";
            }
            else
            {
                return "Item unequipped";
            }
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class EquipLeftHandler : AbstractInputHandler
{
    private Logic _logic;
    public EquipLeftHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.D2)
        {
            var eq = _logic.EquipLeft();
            if (eq)
            {
                return "Item equipped";
            }
            else
            {
                return "Item unequipped";
            }
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class ExitHandler : AbstractInputHandler
{
    private Logic _logic;
    public ExitHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.X)
        {
            _logic.GameEnd = true;
            return "Exiting";
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class DrinkHandler : AbstractInputHandler
{
    private Logic _logic;
    public DrinkHandler(Logic logic)
    {
        _logic = logic;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.U)
        {
            if (_logic.DrinkLogic())
            {
                return "Drinking potion";
            }
            else
            {
                return base.Handle(request);
            }
        }
        else
        {
            return base.Handle(request);
        }
    }
}
class AttackSelectHandler : AbstractInputHandler
{
    private GameState _gameState;
    public AttackSelectHandler(GameState gameState)
    {
        _gameState = gameState;
    }
    public override object? Handle(object request)
    {
        if ((request as ConsoleKey?) == ConsoleKey.C)
        {
            _gameState.Player.ChoseAttackIndex = (_gameState.Player.ChoseAttackIndex + 1) % _gameState.Player.Attacks.Count;
            return "Attack changed";
        }
        else
        {
            return base.Handle(request);
        }
    }
}