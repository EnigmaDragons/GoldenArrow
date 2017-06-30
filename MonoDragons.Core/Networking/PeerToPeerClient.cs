using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Newtonsoft.Json;
using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerClient : IMessenger
    {
        private NetClient _client;
        private List<string> _messages = new List<string>();
        public List<string> GetNewMessages { get { var m = _messages; _messages.Clear(); return m; } }

        public PeerToPeerClient(string url, int port)
        {
            _client = new NetClient(new NetPeerConfiguration("chat") { AutoFlushSendQueue = false });
        }

        public void Init(string url, int port)
        {
            _client.Start();
            NetOutgoingMessage hail = _client.CreateMessage("This is the hail message");
            _client.Connect(url, port, hail);
        }
        
        public void Send(object item)
        {
            NetOutgoingMessage message = _client.CreateMessage(JsonConvert.SerializeObject(item));
            _client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            _client.FlushSendQueue();
        }

        public void Update(TimeSpan delta)
        {
            NetIncomingMessage im;
            while ((im = _client.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                    _messages.Add(im.ReadString());
                _client.Recycle(im);
            }
        }
    }
}
