using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rogue;

namespace TCP;

public class Server
{
    private int _maxClients;
    private int _port;
    private List<TcpClient> _clients;

    private TcpListener _listener;
    private bool _isRunning;
    private readonly object _clientsLock = new object();
    public event Action<string, TcpClient> MessageReceived = delegate { };
    public event Action<TcpClient> ClientDisconnected = delegate { };
    public event Action<TcpClient> ClientConnected = delegate { };


    public Server(int port, int maxClients)
    {
        _port = port;
        _clients = new List<TcpClient>();
        _isRunning = false;
        _listener = new TcpListener(IPAddress.Loopback, port);
        _maxClients = maxClients;
    }

    public void Start()
    {
        _listener.Start();
        _isRunning = true;
        System.Console.WriteLine($"Server starts at port: {_port}");

        Task.Run(AcceptClientsAsync);

    }
    private async Task HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        try
        {
            while (client.Connected && _isRunning == true)
            {
                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytes <= 0)
                {
                    break;
                }

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                MessageReceived?.Invoke(msg, client);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Client handle error: {ex.Message}");
        }
        finally
        {
            DisconnectClient(client);
        }
    }
    private async Task AcceptClientsAsync()
    {
        while (_isRunning)
        {
            try
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                System.Console.WriteLine("New client");

                if (_clients.Count > _maxClients)
                {
                    Send(client, "Server is full");
                    client.Close();
                    continue;
                }

                _clients.Add(client);


                _ = Task.Run(() => HandleClient(client));
            }
            catch (Exception ex)
            {
                if (_isRunning)
                    System.Console.WriteLine($"AcceptClientsAsync error: {ex.Message}");
            }
        }
    }
    private void BroadcastGameState(string message)
    {
        foreach (var client in _clients.ToArray())
        {
            Send(client, message);
        }
    }
    private void DisconnectClient(TcpClient client)
    {
        if (!_clients.Contains(client))
        {
            return;
        }
        _clients.Remove(client);
        client.Close();
    }
    private void Send(TcpClient client, string message)
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Send error: {ex.Message}");

        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener?.Stop();

        foreach (var client in _clients)
        {
            client.Close();
        }

        _clients.Clear();
        System.Console.WriteLine("Server shut down");
    }
}

public class Client
{
    private System.Net.Sockets.TcpClient _client = null!;
    private NetworkStream _stream = null!;
    private bool _connected = false;

    public event Action<string> MessageReceived = delegate { };
    public event Action Disconnected = delegate { };

    public async Task<bool> ConnectAsync(string serverIp, int port)
    {
        try
        {
            _client = new System.Net.Sockets.TcpClient();
            await _client.ConnectAsync(serverIp, port);

            _stream = _client.GetStream();
            _connected = true;

            // Task.Run()
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"ConnectAsync error: {ex.Message}");
            return false;
        }
    }
    public bool SendMessage(string message)
    {
        if (!_connected)
        {
            return false;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _stream.Write(buffer, 0, buffer.Length);
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"SendMessage fail: {ex.Message}");
            Disconnect();
            return false;
        }
    }
    public void Disconnect()
    {
        if (!_connected)
        {
            return;
        }
        _connected = false;

        _stream?.Close();
        _client?.Close();

        Disconnected?.Invoke();
    }
    private async Task ReceiveMessageAsync()
    {
        byte[] buffer = new byte[4096];

        try
        {
            while(_connected && _client.Connected)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                if(bytesRead == 0)
                {
                    break;
                }
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MessageReceived?.Invoke(msg);
            }
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"ReceiveMessageAsync fail: {ex.Message}");
        }
        finally
        {
            Disconnect();
        }
    }


}