using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using MonoDragons.Core.Common;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerClient : INetworker
    {
        private const string ConnectionDenoter = "C";
        private const string NormalDenoter = " ";
        private const string ToEchoDenoter = "E";
        private const string EchoedDenoter = "e";

        private NetClient _client;
        private long _sent;
        private NetConnection _connectionStatus;

        public Action<string> ReceivedCallback { get; set; } = (s) => { };
        public int ConnectionsCount { get; private set; }
        public List<string> ConnectionNames { get; private set; } = new List<string>();
        public string YourName { get; private set; }
        public bool IsFull { get; private set; }
        public long Latency { get; private set; } = -2;
        public Optional<bool> Successful => _connectionStatus.Status == NetConnectionStatus.Connected ? new Optional<bool>(true)
            : _connectionStatus.Status == NetConnectionStatus.Disconnected ? new Optional<bool>(false) : new Optional<bool>();

        public PeerToPeerClient()
        {
            _client = new NetClient(new NetPeerConfiguration("chat") { AutoFlushSendQueue = false });
        }

        public void Init(string url, int port, string name)
        {
            YourName = name;
            _client.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
            _client.Start();
            NetOutgoingMessage hail = _client.CreateMessage(name);
            _connectionStatus = _client.Connect(url, port, hail);
        }
        
        public void Send(object item)
        {
            NetOutgoingMessage message = _client.CreateMessage(ToEchoDenoter + JsonConvert.SerializeObject(item));
            Latency = -1;
            _sent = DateTimeOffset.UtcNow.Ticks;
            _client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            _client.FlushSendQueue();
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _client.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var s = im.ReadString();
                    if (s.Substring(0, 1) == ToEchoDenoter)
                    {
                        _client.SendMessage(_client.CreateMessage("e" + s.Substring(1)), NetDeliveryMethod.ReliableOrdered);
                        _client.FlushSendQueue();
                        ReceivedCallback(s.Substring(1));
                    }
                    else if (s.Substring(0, 1) == EchoedDenoter)
                        Latency = DateTimeOffset.UtcNow.Ticks - _sent;
                    else if (s.Substring(0,1) == NormalDenoter)
                        ReceivedCallback(s.Substring(1));
                    else if (s.Substring(0, 1) == ConnectionDenoter)
                    {
                        var maxConnectionsAndNames = s.Substring(1).Split('\"').ToList();
                        var max = int.Parse(maxConnectionsAndNames[0]);
                        maxConnectionsAndNames.RemoveAt(0);
                        maxConnectionsAndNames.Remove(YourName);
                        ConnectionsCount = maxConnectionsAndNames.Count;
                        ConnectionNames = new List<string>(maxConnectionsAndNames);
                        IsFull = max == ConnectionsCount;
                    }
                }
                _client.Recycle(im);
            }
        }

        public void Dispose()
        {
            _client.Shutdown("Disconnect requested.");
        }
    }
}
