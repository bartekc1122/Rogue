using Rogue;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

public class Player : IEntity, IObserver
{

    public Point? Position { get; set; }
    public char Symbol { get => '\u00B6'; }
    public ConsoleColor Color { get => ConsoleColor.Blue; }
    public Statistic Stats { get; set; }
    public Hands Hands { get; set; }
    public List<AEffect> effects { get; set; }
    [JsonInclude]
    public List<ICombatVisitor> Attacks { get; set; }
    public int ChoseAttackIndex { get; set; }
    [JsonIgnore]
    public int Zindex { get; set; }

    public Inventory Inventory { get; set; }
    public Player()
    {
        Zindex = 3;
        Stats = new Statistic(this);
        Inventory = new Inventory();
        Hands = new Hands();
        effects = new List<AEffect>();
        Attacks = new List<ICombatVisitor> { new NormalAttack(), new MagicAttack(), new StealthAttack() };
        ChoseAttackIndex = 0;
    }
    public IEntity Clone()
    {
        var newPlayer = new Player
        {
            Position = this.Position,
            Stats = new Statistic(null!),
            Inventory = new Inventory(),
            Hands = new Hands(),
            effects = new List<AEffect>(this.effects.Select(e => e.Clone(e))),
            Attacks = new List<ICombatVisitor>(this.Attacks),
            ChoseAttackIndex = this.ChoseAttackIndex,
            Zindex = this.Zindex
        };
        newPlayer.Stats.Player = newPlayer;
        return newPlayer;
    }

    public void Update(ISubject subject)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].TurnUpdate();
            if (effects[i].Duration == 0)
            {
                effects.Remove(effects[i]);
            }
        }
    }
}

public class Statistic
{
    private int _power;
    private int _agility;
    private int _health;
    private int _luck;
    private int _aggro;
    private int _wisdom;
    private int _money;
    public int Power
    {
        get
        {
            int result = _power;
            if (Player?.effects != null)
            {
                foreach (var effect in Player.effects)
                {
                    result += effect.ApplyPower(result);
                }
            }
            return result >= 0 ? result : 0;
        }
        set => _power = value;
    }

    public int Agility
    {
        get
        {
            int result = _agility;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyAgility(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _agility = value;
    }

    public int Health
    {
        get
        {
            int result = _health;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyHealth(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _health = value;
    }

    public int Luck
    {
        get
        {
            int result = _luck;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyLuck(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _luck = value;
    }

    public int Aggro
    {
        get
        {
            int result = _aggro;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyAggro(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _aggro = value;
    }

    public int Wisdom
    {
        get
        {
            int result = _wisdom;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyWisdom(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _wisdom = value;
    }

    public int Money
    {
        get
        {
            int result = _money;
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyMoney(result);
            }
            return result >= 0 ? result : 0;
        }
        set => _money = value;
    }
    [JsonIgnore]
    public Player Player { get; set; }
    public Statistic(Player player, int power = 10, int agility = 15, int health = 50, int luck = 1, int aggro = 3, int wisdom = 10, int money = 0)
    {
        Player = player;
        _power = power;
        _agility = agility;
        _health = health;
        _luck = luck;
        _aggro = aggro;
        _wisdom = wisdom;
        _money = money;
    }
}