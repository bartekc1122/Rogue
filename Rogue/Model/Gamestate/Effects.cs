using System.Text.Json.Serialization;

namespace Rogue;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$effectType")]
[JsonDerivedType(typeof(Strong), "strong_effect")]
[JsonDerivedType(typeof(Luck), "luck_effect")]
[JsonDerivedType(typeof(Aggro), "aggro_effect")]
public abstract class AEffect
{
    [JsonInclude]
    public int Duration { protected set; get; }
    protected AEffect() {}
    public AEffect(int duration)
    {
        Duration = duration;
    }
    virtual public void TurnUpdate()
    {
        Duration--;
    }
    virtual public int ApplyPower(int power)
    {
        return 0;
    }
    virtual public int ApplyAgility(int agility)
    {
        return 0;
    }
    virtual public int ApplyHealth(int health)
    {
        return 0;
    }
    virtual public int ApplyLuck(int luck)
    {
        return 0;
    }
    virtual public int ApplyAggro(int aggro)
    {
        return 0;
    }
    virtual public int ApplyWisdom(int wisdom)
    {
        return 0;
    }
    virtual public int ApplyMoney(int money)
    {
        return 0; ;
    }
    public virtual string MyToString()
    {
        return $"Effect ({Duration})";
    }
    public abstract AEffect Clone(AEffect aEffect);
}

public class Strong : AEffect
{
    public Strong() : base() { }
    [JsonConstructor]
    public Strong(int duration) : base(duration)
    { }
    public override int ApplyPower(int power)
    {
        return 5;
    }
    public override string MyToString()
    {
        return $"Strong Effect ({Duration})";
    }
    public override AEffect Clone(AEffect aEffect)
    {
        return new Strong(this.Duration);
    }
}

public class Luck : AEffect
{
    [JsonInclude]
    private int _luck;
    public Luck() : base() { }
    [JsonConstructor]
    public Luck(int duration) : base(duration)
    {
        _luck = duration;
    }
    public override void TurnUpdate()
    {
        Duration--;
        _luck--;
    }

    public override int ApplyLuck(int luck)
    {
        return _luck;
    }
    public override string MyToString()
    {
        return $"Luck Effect ({Duration})";
    }
    public override AEffect Clone(AEffect aEffect)
    {
        return new Luck(this.Duration);
    }
}
public class Aggro : AEffect
{
    public Aggro() : base() { }
    [JsonConstructor]
    public Aggro(int duration) : base(duration)
    {
    }
    public override void TurnUpdate()
    {
    }

    public override int ApplyAggro(int Aggro)
    {
        return 10;
    }
    public override string MyToString()
    {
        return $"Aggro Effect";
    }
    public override AEffect Clone(AEffect aEffect)
    {
        return new Aggro(this.Duration);
    }
}