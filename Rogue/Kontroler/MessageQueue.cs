using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TCP;

public enum MessageType
{
    addPlayer,
    input,
    deletePlayer
}

public class Message
{
    public object? Content { get; set; }
    public int ClientID { get; set; }
    public MessageType Type;
    public Message(int clientID, object? content, MessageType type)
    {
        ClientID = clientID;
        Content = content;
        Type = type;
    }
}

public class MessageQueue
{
    private readonly Queue<Message> _messageQueue = new Queue<Message>();
    private readonly ManualResetEventSlim _messageAvailable = new ManualResetEventSlim(false);
    private readonly object _processingLock = new object();

    public bool HasMessages {get {lock(_processingLock) {return _messageQueue.Count > 0;}}}
    public void EnqueueMessage(int clientID, object? content, MessageType messageType)
    {

        var message = new Message(clientID, content, messageType);
        lock (_processingLock)
        {
            _messageQueue.Enqueue(message);
            if (_messageQueue.Count == 1)
            {
                _messageAvailable.Set();
            }
        }
    }
    public Message? DequeueMessage()
    {
        lock (_processingLock)
        {
            if(_messageQueue.Count == 0)
            {
                _messageAvailable.Reset();
                return null;
            }
            var msg = _messageQueue.Dequeue();
            if(_messageQueue.Count == 0)
            {
                _messageAvailable.Reset();
            }
            return msg;
        }
    }
    public bool WaitForMessage(int timeOutMs = Timeout.Infinite)
    {
        return _messageAvailable.Wait(timeOutMs);
    }
    public void Stop()
    {
        _messageAvailable.Set();
    }
    public void Dispose()
    {
        _messageAvailable.Dispose();
    }
}