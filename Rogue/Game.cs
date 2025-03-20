using Rogeu;
using Rogue;
using System.ComponentModel;
using System.Drawing;
class Game
{
    private readonly GameState _state;
    private readonly Logic _logic;
    private readonly Renderer _renderer;
    public void run()
    {
        Console.CursorVisible = false;
        _renderer.DrawMap();
        while (true)
        {
            _renderer.DrawEntites();
            _renderer.DrawStats();
            HandleInput();
        }
    }

    public Game()
    {
        DungeonBuilder dungeonBuilder = new DungeonBuilder();
        Director.ConstructClassicDungeon(dungeonBuilder);
        _state = dungeonBuilder.getProduct();
        _logic = new Logic(_state);
        _renderer = new Renderer(_state);
    }


    private void HandleInput()
    {
        var key = Console.ReadKey(true).Key;
        Point newPosition = _state.EntityManager.GetEntityPosition(_state.Player);
        switch (key)
        {
            case ConsoleKey.W:
                newPosition.Y--;
                break;
            case ConsoleKey.S:
                newPosition.Y++;
                break;
            case ConsoleKey.D:
                newPosition.X++;
                break;
            case ConsoleKey.A:
                newPosition.X--;
                break;
            case ConsoleKey.E:
                _logic.TryPickUpItem();
                break;
            case ConsoleKey.J:
                _state.Player.Inventory.MoveCursor(1);
                break;
            case ConsoleKey.K:
                _state.Player.Inventory.MoveCursor(-1);
                break;
            case ConsoleKey.T:
                _logic.TryThrowItem();
                break;
            case ConsoleKey.D1:
                _logic.EquipRight();
                break;
            case ConsoleKey.D2:
                _logic.EquipLeft();
                break;
        }
        _state.EntityManager.MoveEntity(_state.Player, newPosition);
    }
    private bool ValidMove(Point newPosition)
    {
        return _state.Map[newPosition.Y, newPosition.X] != TerrainType.Wall;
    }

}