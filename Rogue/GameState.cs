using System.Drawing;
public enum TerrainType
{
    Wall = '█',
    Floor = ' '
}
public class Constants
{
    public const int MapWidth = 41;
    public const int MapHeight = 21;
}
public class GameState
{
    public TerrainType[,] Map { get; }
    public Player Player { get; set; }
    public EntityManager EntityManager;
    public GameState()
    {
        Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
        Player = new Player();
        EntityManager = new EntityManager(this);
        EntityManager.AddEntity(Player, new Point(1, 1));
    }
}