using System.Runtime.InteropServices;
using System.Text.Json;
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
                            var messageQueue = new MessageQueue();
                            int port = int.Parse(args[i + 1]);
                            var server = new TCP.Server(port, 9);
                            server.MessageReceived += (message, clientID) => { messageQueue.EnqueueMessage(clientID, message, MessageType.input); };
                            server.ClientConnected += (clientID) => { messageQueue.EnqueueMessage(clientID, "Connect", MessageType.addPlayer); };
                            server.ClientDisconnected += (clientID) => { messageQueue.EnqueueMessage(clientID, "Disconnect", MessageType.deletePlayer); };

                            Game game = new Game(messageQueue);

                            server.Start();

                            try
                            {
                                await Task.Run(() => game.run());
                            }
                            finally
                            {
                                server.Stop();
                            }
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
                                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                                    if (keyInfo.Key == ConsoleKey.Q)
                                    {
                                        break;
                                    }
                                    ConsoleKeyInfoDTO dto = new ConsoleKeyInfoDTO(keyInfo);


                                    var jsonMessage = JsonSerializer.Serialize(dto);
                                    client.SendMessage(jsonMessage);
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
