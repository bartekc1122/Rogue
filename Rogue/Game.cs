using Microsoft.Win32;
using Rogue;
using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
public class ConsoleKeyInfoDTO
{
    public char KeyChar { get; set; }
    public ConsoleKey Key { get; set; }
    public ConsoleModifiers Modifiers { get; set; }

    public ConsoleKeyInfoDTO(ConsoleKeyInfo cki)
    {
        KeyChar = cki.KeyChar;
        Key = cki.Key;
        Modifiers = cki.Modifiers;
    }
    public ConsoleKeyInfoDTO() { }
    public ConsoleKeyInfo ToConsoleKeyInfo()
    {
        bool shift = (Modifiers & ConsoleModifiers.Shift) != 0;
        bool alt = (Modifiers & ConsoleModifiers.Alt) != 0;
        bool control = (Modifiers & ConsoleModifiers.Control) != 0;

        return new ConsoleKeyInfo(KeyChar, Key, shift, alt, control);
    }
}


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
            var keyInfo = Console.ReadKey(true);
            ConsoleKeyInfoDTO dto = new ConsoleKeyInfoDTO(keyInfo);
            var jsonMessage = JsonSerializer.Serialize(dto);
            _messageQueue.EnqueueMessage(0, jsonMessage, MessageType.input);
        }
    }
    private void ProcessMessage(Message message)
    {
        switch (message.Type)
        {
            case MessageType.input:
                _logic.SelectPlayer(message.ClientID);

                var receivedData = JsonSerializer.Deserialize<ConsoleKeyInfoDTO>((string)message.Content!);
                var key = receivedData!.ToConsoleKeyInfo();
                var valid = _inputHandler.Handle(key.Key);
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