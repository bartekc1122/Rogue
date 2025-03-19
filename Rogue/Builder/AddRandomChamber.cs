using System.Drawing;

namespace Rogue;

public class AddRandomChamber : IBuildProcedure
{
    private Random _random = new Random();
    int _size;

    public AddRandomChamber(int size)
    {
        _size = size;
    }

    public void Apply(GameState gameState)
    {
        TerrainType[,] map = gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(_size + _random.Next() % (width - _size),
        _size/2 + _random.Next() % (height - _size/2));
        for (int i = middle.Y - _size / 2; i < middle.Y + _size / 2; i++)
            for (int j = middle.X - _size; j < middle.X + _size; j++)
                map[i, j] = TerrainType.Floor;
    }
}