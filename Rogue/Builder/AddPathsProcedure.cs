using System.Drawing;

namespace Rogue;

public class AddPathsProcedure : IBuildProcedure
{
    private Random _random = new Random();

    public AddPathsProcedure()
    {
    }

    public void Apply(GameState gameState)
    {
        TerrainType[,] map = gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);

        Point start = new Point(1, 1);

        GeneratePaths(map, start, width, height);
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
}