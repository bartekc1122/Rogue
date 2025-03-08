using System.Drawing;
public interface IEntity
{
    Point Position { get; set; }
    char Symbol { get; }
    ConsoleColor Color { get; }
}