using System.Drawing;
using System.Transactions;
public class Renderer
{
    private readonly GameState _state;
    private readonly Dictionary<Point, (char Symbol, ConsoleColor color)> _lastFrame = new();
    private readonly List<(string line, ConsoleColor color)> _lastStats = new();
    public Renderer(GameState state) => _state = state;

    public void DrawEntites()
    {
        var currentFrame = new Dictionary<Point, (char Symbol, ConsoleColor color)>();

        foreach (var entity in _state.EntityManager.GetAllEntities().OrderBy(e => e.ZIndex).Reverse())
        {
            currentFrame[entity.Position!.Value] = (entity.Symbol, entity.Color);
        }

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
        var currentStats = new List<(string, ConsoleColor)>();
        CurrentStatsState(currentStats);
        CurrentInventoryState(currentStats);
        CurrentHandsState(currentStats);
        CurrentItemOnFloorState(currentStats);

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
        int lenghtOfLine = 40;
        var toUpdate = new List<(int inx, string line, ConsoleColor color)>();
        for (int i = 0; i < currentStats.Count; i++)
        {
            if (!(i < _lastStats.Count) || currentStats[i].line != _lastStats[i].line)
            {
                toUpdate.Add((i, currentStats[i].line + new string(' ', lenghtOfLine - currentStats[i].line.Length), currentStats[i].color));
                continue;
            }
            // Clearing lines 
            for (int j = currentStats.Count; j < _lastStats.Count; j++)
            {
                toUpdate.Add((j, new string(' ', lenghtOfLine), ConsoleColor.Black));
            }
        }
        return toUpdate;
    }
    public void CurrentInventoryState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        currentStats.Add(($"Inventory:", ConsoleColor.Magenta));
        for (int i = 0; i < _state.Player.Inventory.InventoryCount(); i++)
        {
            var item = _state.Player.Inventory.GetInventory()[i];
            if (item == _state.Player.Inventory.GetSelectedItem())
            {
                currentStats.Add(("-> " + item.ToString(), ConsoleColor.DarkYellow));
            }
            else
            {
                currentStats.Add((item.ToString(), item.Color));
            }
        }
    }
    public void CurrentStatsState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.AddRange(new[]
        {
            (new string('#', 20), ConsoleColor.DarkCyan),
            ($"Money: {_state.Player.Stats.Money}",ConsoleColor.Yellow),
            ($"Power: {_state.Player.Stats.Power}",ConsoleColor.White),
            ($"Agility: {_state.Player.Stats.Agility}",ConsoleColor.White),
            ($"Health: {_state.Player.Stats.Health}",ConsoleColor.White),
            ($"Luck: {_state.Player.Stats.Luck}",ConsoleColor.White),
            ($"Aggresion: {_state.Player.Stats.Aggro}",ConsoleColor.White),
            ($"Wisdom: {_state.Player.Stats.Wisdom}",ConsoleColor.White),
        });
    }

    public void CurrentHandsState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        var Right = _state.Player.Hands.Right;
        var Left = _state.Player.Hands.Left;
        currentStats.Add(("Right hand: " + (Right?.ToString() ?? ""), ConsoleColor.Cyan));
        currentStats.Add(("Left hand: " + (Left?.ToString() ?? ""), ConsoleColor.Cyan));
    }

    public void CurrentItemOnFloorState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        var itemsOnFloor = _state.EntityManager.GetItemsAt(_state.EntityManager.GetEntityPosition(_state.Player));
        if (itemsOnFloor.Any())
        {
            var itemOnFloor = itemsOnFloor.First().ToString();
            currentStats.Add(("Pick up: " + itemOnFloor, ConsoleColor.Yellow));
        }
    }
}