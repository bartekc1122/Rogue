using System.Drawing;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using Rogue;
public class Renderer
{
    private static readonly Lazy<Renderer> _instance = new Lazy<Renderer>(() => new Renderer());
    public static Renderer Instance => _instance.Value;
    private GameState _state = new GameState();
    private readonly Dictionary<Point, (char Symbol, ConsoleColor color)> _lastFrame = new();
    private readonly List<(string line, ConsoleColor color)> _lastStats = new();
    private Player _player = null!;

    public void SetGameState(GameState gameState, int playerID)
    {
        _state = gameState;
        _player = _state.Players[playerID];
    }
    public void DrawEntities()
    {
        if (_state?.EntityManager == null) { return; }
        var currentFrame = new Dictionary<Point, (char Symbol, ConsoleColor color)>();

        foreach (var entity in _state.EntityManager.GetAllEntities().OrderBy(e => e.ZIndex))
        {
            if (entity.Position.HasValue)
            {
                currentFrame[entity.Position!.Value] = (entity.Symbol, entity.Color);
            }
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
    public void DrawMap(Manual manual)
    {
        Console.Clear();
        for (int y = 0; y < _state.Map.GetLength(0); y++)
        {
            for (int x = 0; x < _state.Map.GetLength(1); x++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write((char)_state.Map[y, x]);
            }
        }
        int inx = _state.Map.GetLength(0);
        foreach (var line in manual.GetManual())
        {
            if (inx < Console.WindowHeight)
            {
                Console.SetCursorPosition(0, inx);
                Console.Write(line.PadRight(Console.WindowWidth > Constants.MapWidth ? Constants.MapWidth : Console.WindowWidth - 1));
            }
            inx++;
        }
    }
    public void DrawStats(string lastAction)
    {
        var currentStats = new List<(string, ConsoleColor)>();
        CurrentStatsState(currentStats);
        CurrentEffects(currentStats);
        CurrentInventoryState(currentStats);
        CurrentHandsState(currentStats);
        CurrentItemOnFloorState(currentStats);
        CurrentMonsterNearby(currentStats);
        CurrentLastAction(currentStats);


        var toUpdate = ToUpdateStats(currentStats);

        for (int i = 0; i < toUpdate.Count; i++)
        {
            Console.ForegroundColor = toUpdate[i].color;
            Console.SetCursorPosition(Constants.MapWidth + 1, toUpdate[i].idx);
            System.Console.Write(toUpdate[i].line);
        }
        Console.ResetColor();
        _lastStats.Clear();
        for (int i = 0; i < currentStats.Count; i++)
        {
            _lastStats.Add(currentStats[i]);
        }

    }
    public List<(int idx, string line, ConsoleColor color)> ToUpdateStats(List<(string line, ConsoleColor color)> currentStats)
    {
        int lengthOfLine = 40;
        var toUpdate = new List<(int inx, string line, ConsoleColor color)>();
        for (int i = 0; i < currentStats.Count; i++)
        {
            if (!(i < _lastStats.Count) || currentStats[i].line != _lastStats[i].line)
            {
                toUpdate.Add((i, currentStats[i].line + new string(' ', lengthOfLine - currentStats[i].line.Length), currentStats[i].color));
                continue;
            }
            // Clearing lines 
            for (int j = currentStats.Count; j < _lastStats.Count; j++)
            {
                toUpdate.Add((j, new string(' ', lengthOfLine), ConsoleColor.Black));
            }
        }
        return toUpdate;
    }
    private void CurrentEffects(List<(string, ConsoleColor)> currentStats)
    {
        foreach (var effect in _player.effects)
        {
            currentStats.Add((effect.MyToString(), ConsoleColor.Green));
        }
    }
    private void CurrentLastAction(List<(string, ConsoleColor)> currentStats)
    {
        if (!string.IsNullOrWhiteSpace(_state.LastAction))
        {
            currentStats.Add(($" -> {_state.LastAction} <-", ConsoleColor.White));
        }
    }
    public void CurrentInventoryState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        currentStats.Add(($"Inventory:", ConsoleColor.Magenta));
        foreach (var item in _player.Inventory.GetInventory())
        {
            //var item = _state.Player.Inventory.GetInventory()[i];
            if (item == _player.Inventory.GetSelectedItem())
            {
                currentStats.Add(("-> " + item.MyToString(), ConsoleColor.DarkYellow));
            }
            else
            {
                currentStats.Add((item.MyToString(), item.Color));
            }
        }
    }
    public void CurrentStatsState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.AddRange(new[]
        {
            (new string('#', 20), ConsoleColor.DarkCyan),
            ($"Money: {_player.Stats.Money}",ConsoleColor.Yellow),
            ($"Power: {_player.Stats.Power}",ConsoleColor.White),
            ($"Agility: {_player.Stats.Agility}",ConsoleColor.White),
            ($"Health: {_player.Stats.Health}",ConsoleColor.White),
            ($"Luck: {_player.Stats.Luck}",ConsoleColor.White),
            ($"Aggression: {_player.Stats.Aggro}",ConsoleColor.White),
            ($"Wisdom: {_player.Stats.Wisdom}",ConsoleColor.White),
        });
    }

    public void CurrentHandsState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        var Right = _player.Hands.Right;
        var Left = _player.Hands.Left;
        currentStats.Add(("Right hand: " + (Right?.MyToString() ?? ""), ConsoleColor.Cyan));
        currentStats.Add(("Left hand: " + (Left?.MyToString() ?? ""), ConsoleColor.Cyan));
        currentStats.Add(($"Selected: {_player.Attacks[_player.ChoseAttackIndex].MyGetString()}", ConsoleColor.Cyan));
    }

    public void CurrentItemOnFloorState(List<(string, ConsoleColor)> currentStats)
    {
        currentStats.Add((new string('#', 20), ConsoleColor.DarkCyan));
        var itemsOnFloor = _state.EntityManager.GetItemsAt(_state.EntityManager.GetEntityPosition(_player));
        if (itemsOnFloor.Any())
        {
            var itemOnFloor = itemsOnFloor.First().MyToString();
            currentStats.Add(("Pick up: " + itemOnFloor, ConsoleColor.Yellow));
        }
    }
    public void CurrentMonsterNearby(List<(string, ConsoleColor)> currentStats)
    {
        if (_player.Position == null)
        {
            return;
        }
        int px = _player.Position.Value.X;
        int py = _player.Position.Value.Y;
        for (int dx = -1; dx <= 1; dx++)
        {
            if (dx == 0)
                continue;
            int nx = px + dx;

            if (0 < nx && nx < Constants.MapWidth)
            {
                foreach (var monster in _state.EntityManager.GetMonstersAt(new Point(nx, py)))
                    currentStats.Add(($"-> {monster.Name} is nearby", ConsoleColor.Red));

            }
        }
        for (int dy = -1; dy <= 1; dy++)
        {
            if (dy == 0)
                continue;
            int ny = py + dy;

            if (0 < ny && ny < Constants.MapHeight)
            {
                foreach (var monster in _state.EntityManager.GetMonstersAt(new Point(px, ny)))
                    currentStats.Add(($"-> {monster.Name} is nearby", ConsoleColor.Red));

            }
        }
    }
    public void ClearCMD()
    {
        Console.Clear();
    }
}