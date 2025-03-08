using System.Drawing;
using System.Security.Cryptography;
public class Renderer
{
    private readonly GameState _state;
    private readonly Dictionary<Point, (char Symbol, ConsoleColor color)> _lastFrame = new();
    private readonly List<(string line, ConsoleColor color)> _lastStats = new();
    public Renderer(GameState state) => _state = state;

    public void Draw()
    {
        var currentFrame = new Dictionary<Point, (char Symbol, ConsoleColor color)>();
        currentFrame[_state.Player.Position] = (_state.Player.Symbol, _state.Player.Color);

        var toUpdate = ToUpdateMap(currentFrame);

        foreach (var (pos, col) in toUpdate)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            if (currentFrame.TryGetValue(pos, out var current))
            {
                Console.ForegroundColor = current.color;
                System.Console.Write(current.Symbol);
                Console.ResetColor();
            }
            else
            {
                System.Console.Write((char)_state.Map[pos.Y, pos.X]);
            }
        }
        _lastFrame.Clear();
        foreach (var kv in currentFrame)
            _lastFrame[kv.Key] = kv.Value;
    }
    private List<(Point, (char Symbol, ConsoleColor color))> ToUpdateMap(Dictionary<Point, (char Symbol, ConsoleColor color)> currentFrame)
    {
        var toUpdate = new List<(Point, (char Symbol, ConsoleColor color))>();
        foreach (var kv in currentFrame)
        {
            if (!_lastFrame.TryGetValue(kv.Key, out var val) || val != kv.Value)
            {
                toUpdate.Add((kv.Key, kv.Value));
            }
        }
        foreach (var kv in _lastFrame)
        {
            if (!currentFrame.ContainsKey(kv.Key))
            {
                toUpdate.Add((kv.Key, kv.Value));
            }
        }
        return toUpdate;
    }
    public void DrawMap()
    {
        for (int y = 0; y < _state.Map.GetLength(0); y++)
        {
            for (int x = 0; x < _state.Map.GetLength(1); x++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write((char)_state.Map[y, x]);
            }
        }
    }
    public void DrawStats()
    {
        var currentStats = new List<(string,ConsoleColor)>
        {
            ($"#############################", ConsoleColor.Green),
            ($"Power: {_state.Player.Stats.Power}",ConsoleColor.White),
            ($"Agility: {_state.Player.Stats.Agility}",ConsoleColor.White),
            ($"Health: {_state.Player.Stats.Health}",ConsoleColor.White),
            ($"Luck: {_state.Player.Stats.Luck}",ConsoleColor.White),
            ($"Aggresion: {_state.Player.Stats.Aggro}",ConsoleColor.White),
            ($"Wisdom: {_state.Player.Stats.Wisdom}",ConsoleColor.White),
            ($"#############################",ConsoleColor.Green),
        };


        var toUpdate = ToUpdateStats(currentStats);

        for (int i = 0; i < toUpdate.Count; i++)
        {
            Console.ForegroundColor = toUpdate[i].color;
            Console.SetCursorPosition(Constants.MapWidth, toUpdate[i].idx);
            System.Console.Write(toUpdate[i].line);
            Console.ResetColor();
        }
        _lastStats.Clear();
        for (int i = 0; i < currentStats.Count; i++) 
        {
            _lastStats.Add(currentStats[i]);
        }

    }
    public List<(int idx, string line, ConsoleColor color)> ToUpdateStats(List<(string line, ConsoleColor color)> currentStats)
    {
        var toUpdate = new List<(int inx, string line, ConsoleColor color)>();
        for (int i = 0; i < currentStats.Count; i++)
        {
            if (!(i < _lastStats.Count) || currentStats[i].line != _lastStats[i].line)
            {
                toUpdate.Add((i, currentStats[i].line, currentStats[i].color));
            }
        }
        return toUpdate;
    }
}