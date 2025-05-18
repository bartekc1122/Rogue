using Rogue;
using System.ComponentModel;
using System.Drawing;
class Game
{
    private readonly GameState _state;
    private readonly Logic _logic;
    private readonly Renderer _renderer;
    private IInputHandler _inputHandler;
    private MessageQueue _messageQueue;
    // private ISubject _subject;
    public void run()
    {
        Console.CursorVisible = false;

        Task.Run(() => HandleInput());
        _renderer.DrawMap(_state.manual);
        while (!_logic.GameEnd)
        {

            _renderer.DrawEntities();
            _renderer.DrawStats(_state.LastAction);

            if (_messageQueue.WaitForMessage(5000))
            {
                var msg = _messageQueue.DequeueMessage();
                if (msg != null)
                {
                    ProcessMessage(msg);
                }
            }
            // HandleInput();
            // if (_state.Players.Stats.Health <= 0)
            // {
            //     _logic.GameEnd = true;
            //     Console.Clear();
            //     System.Console.WriteLine("You died...");
            //     Console.ReadKey();
            // }
        }
        _renderer.ClearCMD();
    }

    public Game(MessageQueue messageQueue)
    {
        _messageQueue = messageQueue;
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
        _logic.AddPlayer(0);

        _renderer = Renderer.Instance;
        _renderer.SetGameState(_state);

        // _subject = new Subject();
        // _subject.Attach(_state.Players);
    }


    private void HandleInput()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            _messageQueue.EnqueueMessage(0, key.ToString(), MessageType.input);
        }
    }
    private void ProcessMessage(Message message)
    {
        switch (message.Type)
        {
            case MessageType.input:
                _logic.SelectPlayer(message.ClientID);
                var valid = _inputHandler.Handle((ConsoleKey)char.Parse((string)message.Content!));
                if ((valid as int?) == -1)
                {
                    _state.LastAction = "Invalid key";
                }
                else if ((valid as string) != "")
                {
                    // _subject.Notify();
                    _state.LastAction = (valid as string)!;
                }
                else
                {
                    _state.LastAction = "";
                }
                break;
            case MessageType.addPlayer:
                _logic.AddPlayer(message.ClientID);
                _state.LastAction = "New player!";
                break;
            case MessageType.deletePlayer:
                _logic.DeletePlayer(message.ClientID);
                break;
            default:
                break;
        }
    }
}