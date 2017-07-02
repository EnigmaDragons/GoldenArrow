using System.Collections.Generic;
using Lidgren.Network;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerHost : INetworker
    {
        private const string ConnectionDenoter = "C";
        private const string NormalDenoter = " ";
        private const string ToEchoDenoter = "E";
        private const string EchoedDenoter = "e";

        private NetPeerConfiguration _config { get { return _server.Configuration; } }
        private NetServer _server;
        private long _sent;

        public Action<string> ReceivedCallback { get; set; } = (s) => { };
        public int ConnectionsCount { get { return _server.ConnectionsCount; } }
        public bool IsFull { get { return _config.MaximumConnections == _server.ConnectionsCount; } }
        public long Latency { get; private set; } = -2;

        public void Init(int port, int maxConnections)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port, MaximumConnections = maxConnections });
            _server.Start();
            _server.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.StatusChanged && (NetConnectionStatus)im.ReadByte() == NetConnectionStatus.Connected)
                {
                    SendNewMessage(ConnectionDenoter + _server.ConnectionsCount + "/" + _config.MaximumConnections, _server.Connections);
                }
                else if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var s = im.ReadString();
                    if (s.Substring(0, 1) == ToEchoDenoter)
                    {
                        RelayAndRespondToMessage(im, s);
                        SendNewMessage(EchoedDenoter + s.Substring(1), new List<NetConnection>() { im.SenderConnection });
                    }
                    else if (s.Substring(0, 1) == NormalDenoter)
                        RelayAndRespondToMessage(im, s);
                    else if (s.Substring(0, 1) == EchoedDenoter)
                    {
                        if(Latency == -1)
                            Latency = DateTimeOffset.UtcNow.Ticks - _sent;
                    }
                }
                _server.Recycle(im);
            }
        }

        private void RelayAndRespondToMessage(NetIncomingMessage im, string s)
        {
            List<NetConnection> all = _server.Connections;
            all.Remove(im.SenderConnection);
            SendNewMessage(NormalDenoter + s.Substring(1), all);
            ReceivedCallback(s.Substring(1));
        }

        private void SendNewMessage(string text, List<NetConnection> connections)
        {
            if (connections.Count > 0)
            {
                NetOutgoingMessage message = _server.CreateMessage(text);
                _server.SendMessage(message, connections, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public void Send(object item)
        {
            NetOutgoingMessage om = _server.CreateMessage(ToEchoDenoter + JsonConvert.SerializeObject(item));
            List<NetConnection> all = _server.Connections;
            if (all.Count > 0)
            {
                Latency = -1;
                _sent = DateTimeOffset.UtcNow.Ticks;
                _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public void Dispose()
        {
            _server.Shutdown("Server Shutdown");
        }
    }
}
