using Rogue;
using System.Drawing;
using System.Runtime.CompilerServices;

public class Player : IEntity, IObserver
{

    public Point? Position { get; set; }
    public char Symbol { get => '\u00B6'; }
    public ConsoleColor Color { get => ConsoleColor.Blue; }
    public Statistic Stats;
    public Hands Hands;
    public List<AEffect> effects;
    public int Zindex;

    public Inventory Inventory;
    public Player()
    {
        Zindex = 3;
        Stats = new Statistic(this);
        Inventory = new Inventory();
        Hands = new Hands();
        effects = new List<AEffect>();
    }
    public IEntity Clone()
    {
        return new Player();
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
            foreach (var effect in Player.effects)
            {
                result += effect.ApplyPower(result);
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
    public Player Player { get; set; }
    public Statistic(Player player, int power = 10, int agility = 15, int health = 20, int luck = 1, int aggro = 3, int wisdom = 10, int money = 0)
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