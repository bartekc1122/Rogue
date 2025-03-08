
using System.Drawing;
using System.Runtime.InteropServices;
public enum TerrainType
{
    Wall = '█',
    Floor = ' '
}
public class Constants
{
    public const int MapWidth = 40;
    public const int MapHeight = 20;
}
public class GameState
{
    public TerrainType[,] Map {get;}
    public Player Player {get; set;}

    public GameState()
    {
        Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
        InitializeMap();
        Player = new Player();
    }
    private void InitializeMap()
    {
        for (int y = 0; y < Constants.MapHeight; y++)
        {
            for (int x = 0; x < Constants.MapWidth; x++)
            {
                if (y == 0 || x == 0 || y == Constants.MapHeight - 1 || x == Constants.MapWidth - 1)
                {
                    Map[y, x] = TerrainType.Wall;
                }
                else
                {
                    Map[y, x] = TerrainType.Floor;
                }
            }
        }
        for(int i = 5; i < 15;i++)
        {
            Map[i,9] = TerrainType.Wall;
        }
        for(int i = 5; i < 15;i++)
        {
            Map[i,30] = TerrainType.Wall;
        }
    }
}