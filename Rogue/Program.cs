namespace Rogue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowHeight = 40;
            Game game = new Game();
            game.run();
        }
    }
}
