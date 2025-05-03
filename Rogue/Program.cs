using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TCP;

namespace Rogue
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WindowHeight = 40;

            if (args.Length != 2)
            {
                return;
            }
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--server":
                        if (args.Length > i + 1)
                        {
                            int port = int.Parse(args[i + 1]);
                            var server = new TCP.Server(port, 9);
                            server.MessageReceived += (message, client) =>
                            {
                                System.Console.WriteLine(message);
                            };
                            server.ClientConnected += (client) => { System.Console.WriteLine("New client connected"); };
                            server.ClientDisconnected += (client) => { System.Console.WriteLine("Client disconnected"); };
                            // Game game = new Game();
                            // game.run();
                            server.Start();
                            System.Console.WriteLine("Server start");
                            Console.ReadKey();
                            server.Stop();
                        }
                        break;
                    case "--client":
                        if (args.Length > i + 1)
                        {
                            string addressPort = args[i + 1];
                            var parts = addressPort.Split(":");
                            if (parts.Length != 2)
                            {
                                return;
                            }
                            string address = parts[0];
                            int port = int.Parse(parts[1]);

                            var client = new TCP.Client();
                            client.MessageReceived += (message) => { System.Console.WriteLine(message); };
                            client.Disconnected += () => { System.Console.WriteLine("Disconnected"); };
                            bool connected = await client.ConnectAsync(address, port);

                            if (connected)
                            {
                                while (true)
                                {
                                    string message = Console.ReadLine()!;
                                    if (message == "q")
                                    {
                                        break;
                                    }
                                    client.SendMessage(message);
                                }
                                client.Disconnect();
                            }
                        }
                        else { System.Console.WriteLine("Cannot connect to the server!"); }
                        break;
                }
            }

        }
    }
}
