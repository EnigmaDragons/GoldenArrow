﻿using System.Collections.Generic;
using Lidgren.Network;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System;
using MonoDragons.Core.Common;

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
        private bool _disposed = false;
        private Dictionary<long, string> _connectionIDToName = new Dictionary<long, string>();

        public Action<string> ReceivedCallback { get; set; } = (s) => { };
        public int ConnectionsCount => _server.ConnectionsCount;
        public List<string> ConnectionNames => _server.Connections.Select((c) =>  _connectionIDToName[c.RemoteUniqueIdentifier]).ToList();
        public string YourName { get; private set; }
        public bool IsFull => _config.MaximumConnections == _server.ConnectionsCount;
        public long Latency { get; private set; } = -2;
        public Optional<bool> Successful { get; private set; } = new Optional<bool>();

        public void Init(int port, string name, int maxConnections)
        {
            YourName = name;
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port, MaximumConnections = maxConnections });
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            try
            {
                _server.Start();
            }
            catch
            {
                Successful = new Optional<bool>(false);
            }
            Successful = new Optional<bool>(true);
            _connectionIDToName.Add(_server.UniqueIdentifier, name);
            _server.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
            var t = new Thread(new ThreadStart(ConnectionTracker));
            t.Start();
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.ConnectionApproval)
                {
                    string s = im.ReadString();
                    if (!_connectionIDToName.ContainsValue(s))
                    {
                        _connectionIDToName.Add(im.SenderConnection.RemoteUniqueIdentifier, s);
                        im.SenderConnection.Approve();
                    }
                    else if (_connectionIDToName.ContainsKey(im.SenderConnection.RemoteUniqueIdentifier)
                        && _connectionIDToName[im.SenderConnection.RemoteUniqueIdentifier] == s)
                            im.SenderConnection.Approve();
                    else
                        im.SenderConnection.Deny();

                }
                else if (im.MessageType == NetIncomingMessageType.StatusChanged && (NetConnectionStatus)im.ReadByte() == NetConnectionStatus.Connected)
                {
                    SendNewMessage(GetConnectionMessage(), new List<NetConnection> { im.SenderConnection });
                }
                else if (im.MessageType == NetIncomingMessageType.DebugMessage)
                {
                    var x = im.ReadString();
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

        private string GetConnectionMessage()
        {
            return ConnectionDenoter + _config.MaximumConnections + "\"" + String.Join("\"", ConnectionNames) + "\"" + YourName;
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

        private void ConnectionTracker()
        {
            var lastConnectionCount = 0;
            while (!_disposed)
            {
                if(lastConnectionCount != ConnectionsCount)
                {
                    SendNewMessage(GetConnectionMessage(), _server.Connections);
                    lastConnectionCount = ConnectionsCount;
                }
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            _server.Shutdown("Server Shutdown");
            _disposed = true;
        }
    }
}
