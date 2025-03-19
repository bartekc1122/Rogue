using System.Drawing;

namespace Rogue;

public class AddChamberAt : IBuildProcedure
{
    int _size;
    int _x;
    int _y;
    public AddChamberAt(int size, int x, int y)
    {
        _size = size;
        _x = x;
        _y = y;
    }

    public void Apply(GameState gameState)
    {
        TerrainType[,] map = gameState.Map;
        int width = map.GetLength(1);
        int height = map.GetLength(0);
        Point middle = new Point(_x,_y);
        for (int i = middle.Y - _size / 2; i < middle.Y + _size / 2; i++)
            for (int j = middle.X - _size; j < middle.X + _size; j++)
                map[i, j] = TerrainType.Floor;
    }
}