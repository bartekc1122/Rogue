using System.Drawing;

namespace Rogue;

public class AddMainChamber : IBuildProcedure
{
    private Random _random = new Random();
    int _size;

    public AddMainChamber(int size)
    {
        _size = size;
    }

    public void Apply(GameState gameState)
    {
        TerrainType[,] map = gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(width / 2 + 1, height / 2 + 1);
        for (int i = middle.Y - _size / 2; i < middle.Y + _size / 2; i++)
            for (int j = middle.X - _size; j < middle.X + _size; j++)
                map[i, j] = TerrainType.Floor;
    }
}