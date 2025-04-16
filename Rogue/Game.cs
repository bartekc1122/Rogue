using Rogue;
using System.ComponentModel;
using System.Drawing;
class Game
{
    private readonly GameState _state;
    private readonly Logic _logic;
    private readonly Renderer _renderer;
    private IInputHandler _inputHandler;
    private ISubject _subject;
    public void run()
    {
        Console.CursorVisible = false;
        _renderer.DrawMap(_state.manual);
        while (!_logic.GameEnd)
        {
            _renderer.DrawEntities();
            _renderer.DrawStats(_state.LastAction);
            HandleInput();
        }
        _renderer.ClearCMD();
    }

    public Game()
    {
        DungeonBuilder dungeonBuilder = new DungeonBuilder();
        ManualBuilder manualBuilder = new ManualBuilder();
        Director.ConstructClassicDungeon(dungeonBuilder);

        _state = dungeonBuilder.getProduct();
        Director.ConstructClassicDungeon(manualBuilder);
        _state.manual = manualBuilder.getProduct();
        _logic = new Logic(_state);

        InputBuilder inputBuilder = new InputBuilder(_state, _logic);
        Director.ConstructClassicDungeon(inputBuilder);
        _inputHandler = inputBuilder.GetProduct();

        _renderer = Renderer.Instance;
        _renderer.SetGameState(_state);

        _subject = new Subject();
        _subject.Attach(_state.Player);
    }


    private void HandleInput()
    {
        var key = Console.ReadKey(true).Key;
        var valid = _inputHandler.Handle(key);
        if((valid as int?) == -1)
        {
            _state.LastAction = "Invalid key";
        }
        else if((valid as string) != "")
        {
            _subject.Notify();
            _state.LastAction = (valid as string)!;
        }
        else
        {
            _state.LastAction = "";
        }
    }
    private bool ValidMove(Point newPosition)
    {
        return _state.Map[newPosition.Y, newPosition.X] != TerrainType.Wall;
    }

}