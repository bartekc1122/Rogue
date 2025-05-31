using Rogue;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TimidBehavior), "TimidBehavior")]
[JsonDerivedType(typeof(AggressiveBehavior), "Aggressive")]
[JsonDerivedType(typeof(PassiveBehavior), "Passive")]
public interface IBehavior
{
    Point? MoveToNewPosition(IMonster monster);

}