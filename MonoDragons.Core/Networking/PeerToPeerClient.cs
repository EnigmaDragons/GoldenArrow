using System;
using System.Collections.Generic;
using Lidgren.Network;
using Newtonsoft.Json;
using System.Threading;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerClient : INetworker
    {
        private NetClient _client;
        public Action<string> ReceivedCallback { get; set; } = (s) => { };

        public PeerToPeerClient()
        {
            _client = new NetClient(new NetPeerConfiguration("chat") { AutoFlushSendQueue = false });
        }

        public void Init(string url, int port)
        {
            _client.Start();
            NetOutgoingMessage hail = _client.CreateMessage("This is the hail message");
            _client.Connect(url, port, hail);
            _client.RegisterReceivedCallback(new SendOrPostCallback((a) => GetNewMessages()));
        }
        
        public void Send(object item)
        {
            NetOutgoingMessage message = _client.CreateMessage(JsonConvert.SerializeObject(item));
            _client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            _client.FlushSendQueue();
        }

        private void GetNewMessages()
        {
            NetIncomingMessage im;
            while ((im = _client.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                    ReceivedCallback(im.ReadString());
                _client.Recycle(im);
            }
        }

        public void Dispose()
        {
            _client.Shutdown("Disconnect requested.");
        }
    }
}
