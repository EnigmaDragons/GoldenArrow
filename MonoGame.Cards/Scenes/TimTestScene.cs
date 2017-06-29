using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Cards;
using MonoGame.Cards.Decks;
using Lidgren.Network;
using System.Threading;

namespace MonoGame.Cards.Scenes
{
    public class TimTestScene : IScene
    {
        private static NetClient _client;
        private bool ConnectedAsClient = false;
        private static NetServer _server;
        private bool ConnectedAsHost = false;

        public void Init()
        {
            ConnectAsHost(14000);
        }

        public void ConnectAsClient(string url, int port)
        {
            _client = new NetClient(new NetPeerConfiguration("chat") { AutoFlushSendQueue = false });
            _client.Start();
            NetOutgoingMessage hail = _client.CreateMessage("This is the hail message");
            _client.Connect(url, port, hail);
            ConnectedAsClient = true;
        }
        
        public void ConnectAsHost(int port)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port });
            _server.Start();
            ConnectedAsHost = true;
        } 

        private void RespondToMessage(string message)
        {

        }

        private void RespondToMessagesAsClient(object peer)
        {
            NetIncomingMessage im;
            while ((im = _client.ReadMessage()) != null)
            {
                RespondToMessage(im.ReadString());
                _client.Recycle(im);
            }
        }

        private void RespondToMessagesAsHost()
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var chat = im.ReadString();
                    List<NetConnection> all = _server.Connections;
                    all.Remove(im.SenderConnection);

                    if (all.Count > 0)
                    {
                        NetOutgoingMessage om = _server.CreateMessage();
                        om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                        _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                    }
                    RespondToMessage(chat);
                }
                _server.Recycle(im);
            }
        }

        public void Send(string text)
        {
            if (ConnectedAsClient)
            {
                NetOutgoingMessage om = _client.CreateMessage(text);
                _client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
                _client.FlushSendQueue();
            }
            else
            {
                NetOutgoingMessage om = _server.CreateMessage("Host said: " + text);
                List<NetConnection> all = _server.Connections;
                _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public void Draw()
        {
        }

        public void Update(TimeSpan delta)
        {
            if (ConnectedAsClient)
                RespondToMessagesAsClient(0);
            if (ConnectedAsHost)
                RespondToMessagesAsHost();
        }
    }
}
