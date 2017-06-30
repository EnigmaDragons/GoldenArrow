using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Newtonsoft.Json;

namespace MonoDragons.Core.Networking
{
    public class PeerToPeerHost : IMessenger
    {
        private static NetServer _server;
        private List<string> _messages = new List<string>();
        public List<string> GetNewMessages { get { var m = _messages; _messages.Clear(); return m; } }

        public void Init(int port)
        {
            _server = new NetServer(new NetPeerConfiguration("chat") { Port = port });
            _server.Start();
        }

        public void Send(object item)
        {
            NetOutgoingMessage om = _server.CreateMessage(JsonConvert.SerializeObject(item));
            List<NetConnection> all = _server.Connections;
            _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void Update(TimeSpan delta)
        {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var s = im.ReadString();
                    _messages.Add(s);
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
        }
    }
}
