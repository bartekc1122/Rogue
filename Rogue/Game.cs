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
        Point newPosition = _state.Player.Position;
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

        }
        Move(newPosition);
    }
    private bool ValidMove(Point newPosition)
    {
        return _state.Map[newPosition.Y, newPosition.X] != TerrainType.Wall;
    }
    public void Move(Point position)
    {
        if(!ValidMove(position))
            return; 
        _state.Player.Position = position;
    }
}