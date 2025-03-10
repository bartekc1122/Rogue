using System.Drawing;
class Game
{
    private readonly GameState _state;
    private readonly Renderer _renderer;
    public void run()
    {
        Console.CursorVisible = false;
        _renderer.DrawMap();
        while (true)
        {
            _renderer.Draw();
            _renderer.DrawStats();
            HandleInput();
        }
    }

    public Game()
    {
        _state = new GameState();
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
                _state.TryPickUpItem();
                break;
            case ConsoleKey.J:
                _state.Player.Inventory.MoveCursor(1);
                break;
            case ConsoleKey.K:
                _state.Player.Inventory.MoveCursor(-1);
                break;
            case ConsoleKey.T:
                _state.TryThrowItem();
                break;
            case ConsoleKey.D1:
                _state.EquipRight();
                break;
            case ConsoleKey.D2:
                _state.EquipLeft();
                break;
        }
        _state.EntityManager.MoveEntity(_state.Player, newPosition);
    }
    private bool ValidMove(Point newPosition)
    {
        return _state.Map[newPosition.Y, newPosition.X] != TerrainType.Wall;
    }

}