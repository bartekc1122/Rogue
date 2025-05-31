using System.Drawing;
using Rogue;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    [JsonInclude]
    public Manual manual { get; set; }
    public string LastAction { get; set; }
    [JsonIgnore]
    public TerrainType[,] Map { get; private set; }
    public TerrainType[][]? SerializableMap
    {
        get
        {
            if (Map == null) return null;
            var rows = Map.GetLength(0);
            var cols = Map.GetLength(1);
            var jaggedArray = new TerrainType[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new TerrainType[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = Map[i, j];
                }
            }
            return jaggedArray;
        }
        set
        {
            if (value == null)
            {
                this.Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
                return;
            }
            var rows = value.Length;
            if (rows == 0)
            {
                this.Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
                return;
            }
            var cols = value[0].Length;
            this.Map = new TerrainType[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (value[i] != null && j < value[i].Length)
                        this.Map[i, j] = value[i][j];
                    else
                        this.Map[i, j] = TerrainType.Floor;
                }
            }
        }
    }
    public Dictionary<int, Player> Players { get { return _players; } set => _players = value; }
    private Dictionary<int, Player> _players { get; set; }
    [JsonIgnore]
    public EntityManager EntityManager { get; set; }
    public List<IEntity>? AllEntitiesForSerialization { get; set; }
    public GameState()
    {
        Map = new TerrainType[Constants.MapHeight, Constants.MapWidth];
        // Player = new Player();
        _players = new Dictionary<int, Player>();
        EntityManager = new EntityManager(this);
        // EntityManager.AddEntity(Player, new Point(1, 1));
        manual = new Manual();
        LastAction = "";
        AllEntitiesForSerialization = new List<IEntity>();
    }
    public void PrepareForSerialization()
    {
        if (EntityManager != null)
        {
            AllEntitiesForSerialization = EntityManager.GetAllEntities()
                                                     .ToList();
        }
    }

    public void InitializeAfterDeserialization()
    {
        this.EntityManager = new EntityManager(this);
        if (this.AllEntitiesForSerialization == null)
        {
            return;
        }
        foreach (var entity in this.AllEntitiesForSerialization)
        {
            if (entity.Position.HasValue)
            {
                var position = entity.Position.Value;
                this.EntityManager.AddEntity(entity, position);
            }
            if (entity is IMonster monster)
            {
                if (monster.Behavior is AggressiveBehavior aggressiveBehavior)
                {
                    aggressiveBehavior.GameState = this;
                }
            }
        }

        if (this.Players != null)
        {
            foreach (var player in this.Players.Values)
            {
                if (player.Stats != null) player.Stats.Player = player;

                if (player.Attacks == null || player.Attacks.Count == 0)
                {
                    player.Attacks = new List<ICombatVisitor> { new NormalAttack(), new MagicAttack(), new StealthAttack() };
                }
            }
        }
    }
}