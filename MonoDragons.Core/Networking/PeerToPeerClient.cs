using System;
using System.Collections.Generic;
using Lidgren.Network;
using Newtonsoft.Json;
using System.Threading;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerClient : INetworker
    {
        private const string ConnectionDenoter = "C";
        private const string NormalDenoter = " ";
        private const string PingDenoter = "P";

        private NetClient _client;
        public Action<string> ReceivedCallback { get; set; } = (s) => { };

        public bool IsFull { get; private set; }

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
            NetOutgoingMessage message = _client.CreateMessage(NormalDenoter + JsonConvert.SerializeObject(item));
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
                    if (s.Substring(0, 1) == NormalDenoter)
                        ReceivedCallback(s.Substring(1));
                    else if (s.Substring(0, 1) == ConnectionDenoter)
                    {
                        var index = s.IndexOf("/");
                        if (int.Parse(s.Substring(1, index - 1)) == int.Parse(s.Substring(index + 1)))
                            IsFull = true;
                        else
                            IsFull = false;
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
