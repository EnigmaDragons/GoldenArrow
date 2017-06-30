﻿using System.Collections.Generic;
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
        private static NetServer _server;
        //private ConcurrentDictionary<long, string> _messages = new ConcurrentDictionary<long, string>();
        public Action<string> ReceivedCallback { get; set; } = (s) => { };

        public void Init(int port)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port });
            _server.Start();
            _server.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var s = im.ReadString();
                    List<NetConnection> all = _server.Connections;
                    all.Remove(im.SenderConnection);

                    if (all.Count > 0)
                    {
                        NetOutgoingMessage message = _server.CreateMessage(s);
                        _server.SendMessage(message, all, NetDeliveryMethod.ReliableOrdered, 0);
                    }
                    ReceivedCallback(s);
                }
                _server.Recycle(im);
            }
        }

        public void Send(object item)
        {
            NetOutgoingMessage om = _server.CreateMessage(JsonConvert.SerializeObject(item));
            List<NetConnection> all = _server.Connections;
            if(all.Count > 0)
                _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
        }

        /*public List<string> GetNewMessages()
        {
            var m = _messages;
            _messages.Clear();
            return m.Values.ToList();
        }*/

        /*private void Start(int port)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port });
            _server.Start();
            NetIncomingMessage im;
            var i = 0;
            while (true)
            {
                while ((im = _server.ReadMessage()) != null)
                {
                    if (im.MessageType == NetIncomingMessageType.Data)
                    {
                        var s = im.ReadString();
                        _messages.TryAdd(i++, s);
                        List<NetConnection> all = _server.Connections;
                        all.Remove(im.SenderConnection);

                        if (all.Count > 0)
                        {
                            NetOutgoingMessage message = _server.CreateMessage(s);
                            _server.SendMessage(message, all, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                    }
                    _server.Recycle(im);
                }
                Thread.Sleep(1);
            }
        }*/
    }
}
