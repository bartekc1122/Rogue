using System.Drawing;
using System.Text.Json.Serialization;
using Rogue;

public class PassiveBehavior : IBehavior
{
    [JsonConstructor]
    public PassiveBehavior()
    {}
    public Point? MoveToNewPosition(IMonster monster)
    {
        return null;
    }
}