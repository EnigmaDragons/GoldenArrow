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
        private const string PingDenoter = "P";

        private NetPeerConfiguration _config { get { return _server.Configuration; } }
        private NetServer _server;
        public Action<string> ReceivedCallback { get; set; } = (s) => { };
        public bool IsFull { get { return _config.MaximumConnections == _server.ConnectionsCount; } }

        public void Init(int port, int maxConnections)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port, MaximumConnections = maxConnections });
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            _server.Start();
            _server.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.ConnectionApproval && _config.MaximumConnections != _server.ConnectionsCount)
                {
                    im.SenderConnection.Approve();
                }
                else if (im.MessageType == NetIncomingMessageType.StatusChanged && (NetConnectionStatus)im.ReadByte() == NetConnectionStatus.Connected)
                {
                    List<NetConnection> all = _server.Connections;
                    if (all.Count > 0)
                    {
                        NetOutgoingMessage message = _server.CreateMessage(ConnectionDenoter + _server.ConnectionsCount + "/"
                            + _config.MaximumConnections);
                        _server.SendMessage(message, all, NetDeliveryMethod.ReliableOrdered, 0);
                    }
                }
                else if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var s = im.ReadString();
                    if (s.Substring(0, 1) == NormalDenoter)
                    {
                        List<NetConnection> all = _server.Connections;
                        all.Remove(im.SenderConnection);

                        if (all.Count > 0)
                        {
                            NetOutgoingMessage message = _server.CreateMessage(s);
                            _server.SendMessage(message, all, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        ReceivedCallback(s.Substring(1));
                    }
                }
                _server.Recycle(im);
            }
        }

        public void Send(object item)
        {
            NetOutgoingMessage om = _server.CreateMessage(NormalDenoter + JsonConvert.SerializeObject(item));
            List<NetConnection> all = _server.Connections;
            if(all.Count > 0)
                _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void Dispose()
        {
            _server.Shutdown("Server Shutdown");
        }
    }
}
