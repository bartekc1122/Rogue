using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using TCP;

namespace Rogue;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WindowHeight = 40;
        if (args.Length == 0)
        {
            args = new string[] { "--server", "7777" };
        }
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

                        Game game = new Game(messageQueue, server);

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
                        int myClientID = -1;
                        Renderer renderer = Renderer.Instance;
                        GameState? clientGameState = null;
                        client.MessageReceived += (message) =>
                        {
                            if (message.StartsWith("CLIENT_ID:"))
                            {
                                if (int.TryParse(message.Substring("CLIENT_ID:".Length), out int receivedID))
                                {
                                    myClientID = receivedID;
                                }
                            }
                            else if (message == "SERVER_FULL")
                            {
                                client.Disconnect();
                            }
                            else
                            {
                                var receivedState = JsonSerializer.Deserialize<GameState>(message, Game.DefaultJsonSerializerOptions);
                                if (clientGameState == null)
                                {

                                    receivedState!.InitializeAfterDeserialization();
                                    clientGameState = receivedState;
                                    if (myClientID != -1 && clientGameState.Players.ContainsKey(myClientID))
                                    {
                                        renderer.SetGameState(clientGameState, myClientID);
                                        renderer.DrawMap(receivedState.manual);
                                    }

                                }
                                receivedState!.InitializeAfterDeserialization();
                                clientGameState = receivedState;
                                if (myClientID != -1 && clientGameState.Players.ContainsKey(myClientID))
                                {
                                    renderer.SetGameState(clientGameState, myClientID);
                                    renderer.DrawEntities();
                                    renderer.DrawStats(clientGameState.LastAction);
                                }

                            }
                        };
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
                            Console.Clear();
                        }
                    }
                    else { System.Console.WriteLine("Cannot connect to the server!"); }
                    break;
            }
        }

    }
}

