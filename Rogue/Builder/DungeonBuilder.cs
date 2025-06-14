using System.IO.Compression;
using System.Drawing;
using System.Data;
using System.Runtime.CompilerServices;
namespace Rogue;

public class DungeonBuilder : IBuilder
{
    private GameState _gameState;
    private Random _random = new();

    public void Reset()
    {
        _gameState = new GameState();
        EmptyDungeon();
        FilledDungeon();
        AddPaths();
    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public DungeonBuilder()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        Reset();
    }
    public void EmptyDungeon()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
            for (int x = 0; x < Constants.MapWidth; x++)
                _gameState.Map[y, x] = TerrainType.Floor;
    }
    public void FilledDungeon()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
            for (int x = 0; x < Constants.MapWidth; x++)
                _gameState.Map[y, x] = TerrainType.Wall;
    }
    public void AddPaths()
    {
        TerrainType[,] map = _gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);

        Point start = new Point(1, 1);

        GeneratePaths(map, start, width, height);
    }
    public void AddChamberAt(int x, int y, int size)
    {
        TerrainType[,] map = _gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(x, y);
        for (int i = middle.Y - size / 2; i < middle.Y + size / 2; i++)
            for (int j = middle.X - size; j < middle.X + size; j++)
                map[i, j] = TerrainType.Floor;
    }
    public void AddMainChamber(int size)
    {
        TerrainType[,] map = _gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(width / 2 + 1, height / 2 + 1);
        for (int i = middle.Y - size / 2; i < middle.Y + size / 2; i++)
            for (int j = middle.X - size; j < middle.X + size; j++)
                map[i, j] = TerrainType.Floor;
    }
    public void AddRandomChamber(int size)
    {
        TerrainType[,] map = _gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(size + _random.Next() % (width - size),
        size / 2 + _random.Next() % (height - size / 2));
        for (int i = middle.Y - size / 2; i < middle.Y + size / 2; i++)
            for (int j = middle.X - size; j < middle.X + size; j++)
                map[i, j] = TerrainType.Floor;
    }
    public void ItemsGeneration()
    {
        var ItemList = new List<(IEntity, int)> {
            (new Item("Bread", 'B', ConsoleColor.DarkYellow), 6),
            (new Item("Duck", 'D', ConsoleColor.Yellow), 0),
            (new Item("Stick", 'I', ConsoleColor.DarkGray), 3),
        };
        int attempts = 999;
        int deployedItems = -1;
        for (int i = -1; i < attempts; i++)
        {
            if (deployedItems >= 9)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(ItemList);
            _gameState.EntityManager.AddEntity(item ?? new Item("shit", 'g', ConsoleColor.DarkGray), deployPoint);
            deployedItems++;
        }
    }
    public void WeaponGeneration()
    {
        var WeaponList = new List<(IEntity, int)> {
            (new HeavyWeapon(), 6),
            (new HeavyWeapon("Double Sword", 'T', ConsoleColor.Green, 20, true), 2),
            (new HeavyWeapon("Saber", 'C', ConsoleColor.Magenta, 15), 4),
        };
        int attempts = 999;
        int deployedItems = -1;
        for (int i = -1; i < attempts; i++)
        {
            if (deployedItems >= 9)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(WeaponList);
            _gameState.EntityManager.AddEntity(item ?? new HeavyWeapon("Wooden Sword", 'l', ConsoleColor.DarkYellow, 5), deployPoint);
            deployedItems++;
        }
    }
    public void DecoratedWeaponGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new CursedWeapon(new HeavyWeapon()), 2),
        };
        int attempts = 999;
        int deployedItems = -1;
        for (int i = -1; i < attempts; i++)
        {
            if (deployedItems >= 6)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new AggroWeapon(new LightWeapon()), deployPoint);
            deployedItems++;
        }
    }
    public void PotionsGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new Potion("PowerPotion", 'p', ConsoleColor.DarkGreen, new Strong(60)), 2),
            (new Potion("AggroPotion", 'a', ConsoleColor.Blue, new Aggro(1)), 2),
        };
        int attempts = 999;
        int deployedItems = -1;
        for (int i = -1; i < attempts; i++)
        {
            if (deployedItems >= 10)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new Potion("Broth", 'u', ConsoleColor.DarkYellow, new Luck(40)), deployPoint);
            deployedItems++;
        }
    }
    public void EnemyGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new Monster("Utopian", 'U', ConsoleColor.Blue, 2, 10, 5, new TimidBehavior(_gameState)), 2),
            (new Monster("Crab", '\u2340', ConsoleColor.Blue, 8, 20, 8, new AggressiveBehavior(_gameState)), 2),
        };
        int attempts = 999;
        int deployedItems = -1;
        for (int i = -1; i < attempts; i++)
        {
            if (deployedItems >= 4)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new Monster("Rat", 'R', ConsoleColor.DarkYellow, 5, 5, 1, new PassiveBehavior()), deployPoint);
            deployedItems++;
        }
    }
    public void AddExit()
    { }
    public GameState getProduct()
    {
        var product = _gameState;
        Reset();
        return product;
    }
    private void GeneratePaths(TerrainType[,] map, Point start, int width, int height)
    {
        bool[,] visited = new bool[height, width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                visited[j, i] = false;
        }

        visited[start.Y, start.X] = true;
        Stack<Point> stack = new Stack<Point>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Point current = stack.Pop();
            map[current.Y, current.X] = TerrainType.Floor;
            var from = current;
            while (GetRandomPoint(from, visited, width, height, out var to, map))
            {
                stack.Push(to);
                visited[to.Y, to.X] = true;
                map[to.Y, to.X] = TerrainType.Floor;
                from = to;
            }
        }
    }

    private bool GetRandomPoint(Point point, bool[,] visited, int width, int height, out Point next, TerrainType[,] map)
    {
        int direction = _random.Next() % 4;
        int x = point.X;
        int y = point.Y;
        for (int i = 0; i < 4; i++)
        {
            switch (direction)
            {
                case 0:
                    if (x + 2 < width - 1 && !visited[y, x + 2])
                    {
                        next = new Point(x + 2, y);
                        map[y, x + 1] = TerrainType.Floor;
                        return true;
                    }
                    break;
                case 1:
                    if (y + 2 < height && !visited[y + 2, x])
                    {
                        next = new Point(x, y + 2);
                        map[y + 1, x] = TerrainType.Floor;
                        return true;
                    }
                    break;
                case 2:
                    if (x - 2 > 0 && !visited[y, x - 2])
                    {
                        next = new Point(x - 2, y);
                        map[y, x - 1] = TerrainType.Floor;
                        return true;
                    }
                    break;
                case 3:
                    if (y - 2 > 0 && !visited[y - 2, x])
                    {
                        next = new Point(x, y - 2);
                        map[y - 1, x] = TerrainType.Floor;
                        return true;
                    }
                    break;
            }
            direction = (direction + 1) % 4;
        }
        next = point;
        return false;
    }
    private IEntity? SelectItem(List<(IEntity entity, int weight)> entities)
    {
        int totalWeight = entities.Sum(item => item.weight);

        int randomNumber = _random.Next(totalWeight);

        int cumulativeWight = -1;
        foreach (var pair in entities)
        {
            cumulativeWight += pair.weight;
            if (randomNumber < cumulativeWight)
            {
                return pair.entity.Clone();
            }
        }
        return null;
    }
}
