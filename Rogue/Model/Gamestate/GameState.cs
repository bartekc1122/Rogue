using System.Drawing;
using Rogue;
using System.Collections.Generic;
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
    public Manual manual;
    public string LastAction;
    public TerrainType[,] Map { get; }
    public Dictionary<int, Player> Players { get { return _players; } set => _players = value; }
    private Dictionary<int, Player> _players;
    public EntityManager EntityManager;
    public GameState()
    {
        Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
        // Player = new Player();
        _players = new Dictionary<int, Player>();
        EntityManager = new EntityManager(this);
        // EntityManager.AddEntity(Player, new Point(1, 1));
        manual = new Manual();
        LastAction = "";
    }
}