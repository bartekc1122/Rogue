using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Rogue;
public class TimidBehavior : IBehavior
{
    [JsonIgnore]
    public GameState GameState { get; set; }
    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public TimidBehavior()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {

    }
    public TimidBehavior(GameState gameState)
    {
        GameState = gameState;
    }
    public Point? MoveToNewPosition(IMonster monster)
    {
        Point monsterPoint = GameState.EntityManager.GetEntityPosition(monster);
        Point bestPoint = monsterPoint;
        var currentDistance = NearestPlayerDistance(monsterPoint);
        if (currentDistance > 10)
        {
            return null;
        }

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0 && i == 0 || (j != 0 && i != 0))
                {
                    continue;
                }
                var possiblePoint = new Point(monsterPoint.X + i, monsterPoint.Y + j);
                if (!GameState.EntityManager.IsPositionValid(possiblePoint))
                {
                    continue;
                }
                var possibleDistance = NearestPlayerDistance(possiblePoint);
                if (possibleDistance > currentDistance)
                {
                    bestPoint = possiblePoint;
                }
            }
        }
        return bestPoint;
    }
    private double distance(Point a, Point b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
    public double NearestPlayerDistance(Point possibleMonsterPosition)
    {
        double minDistance = double.MaxValue;
        foreach (var (nr, player) in GameState.Players)
        {
            var distanceToPlayer = distance(GameState.EntityManager.GetEntityPosition(player), possibleMonsterPosition);
            minDistance = distanceToPlayer < minDistance ? distanceToPlayer : minDistance;
        }
        return minDistance;
    }
}