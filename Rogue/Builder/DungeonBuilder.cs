using System.IO.Compression;
using System.Drawing;
using System.Data;
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
        int attemps = 999;
        int deplyedItems = -1;
        for (int i = -1; i < attemps; i++)
        {
            if (deplyedItems >= 9)
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
            deplyedItems++;
        }
    }
    public void WeaponGeneration()
    {
        var WeaponList = new List<(IEntity, int)> {
            (new Weapon("Sword", 't', ConsoleColor.Gray, 10), 6),
            (new Weapon("Double Sword", 'T', ConsoleColor.Green, 20, true), 2),
            (new Weapon("Saber", 'C', ConsoleColor.Magenta, 15), 4),
        };
        int attemps = 999;
        int deplyedItems = -1;
        for (int i = -1; i < attemps; i++)
        {
            if (deplyedItems >= 9)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(WeaponList);
            _gameState.EntityManager.AddEntity(item ?? new Weapon("Wooden Sword", 'l', ConsoleColor.DarkYellow, 5), deployPoint);
            deplyedItems++;
        }
    }
    public void DecoratedWeaponGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new CursedWeapon(new Weapon("Sword", 't', ConsoleColor.Gray, 10)), 2),
            (new AggroWeapon(new Weapon("Double Sword", 'T', ConsoleColor.Green, 20, true)), 2),
            (new CursedWeapon(new Weapon("Saber", 'C', ConsoleColor.Magenta, 15)), 3),
        };
        int attemps = 999;
        int deplyedItems = -1;
        for (int i = -1; i < attemps; i++)
        {
            if (deplyedItems >= 6)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new AggroWeapon(new Weapon("Wooden Sword", 'l', ConsoleColor.DarkYellow, 5)), deployPoint);
            deplyedItems++;
        }
    }
    public void PotionsGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new Potion("PoisonPotion", 'p', ConsoleColor.DarkGreen), 2),
            (new Potion("HealthPotion", 'h', ConsoleColor.DarkRed), 2),
        };
        int attemps = 999;
        int deplyedItems = -1;
        for (int i = -1; i < attemps; i++)
        {
            if (deplyedItems >= 4)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new Potion("Broth", 'u', ConsoleColor.DarkYellow), deployPoint);
            deplyedItems++;
        }
    }
    public void EnemyGeneration()
    {
        var DecoratedWeaponList = new List<(IEntity, int)> {
            (new Monster("Utopian", 'U', ConsoleColor.Blue, 4, 10), 2),
            (new Monster("Rock", '\u2340', ConsoleColor.Blue, 24, 34), 2),
        };
        int attemps = 999;
        int deplyedItems = -1;
        for (int i = -1; i < attemps; i++)
        {
            if (deplyedItems >= 4)
            {
                break;
            }
            Point deployPoint = new Point(_random.Next(Constants.MapWidth), _random.Next(Constants.MapHeight));
            if (_gameState.Map[deployPoint.Y, deployPoint.X] == TerrainType.Wall)
            {
                continue;
            }
            var item = SelectItem(DecoratedWeaponList);
            _gameState.EntityManager.AddEntity(item ?? new Monster("Rat", 'R', ConsoleColor.DarkYellow, 5 ,5), deployPoint);
            deplyedItems++;
        }
    }
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
