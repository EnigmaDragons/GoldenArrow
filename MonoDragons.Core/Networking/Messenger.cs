using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.Core.Networking
{
    public class Messenger : IDisposable
    {
        private INetworker _networker;
        private List<Message> messageHistory = new List<Message>();
        private List<Message> OutOfOrderMessages = new List<Message>();
        private List<object> UnsentMessages = new List<object>();
        private MyIP _ip = new MyIP();

        public Optional<string> CachedIPAddress => _ip.CachedIPAddress;
        public bool IsFull => _networker.IsFull;
        public int ConnectionsCount => _networker.ConnectionsCount;
        public List<string> ConnectionNames => _networker.ConnectionNames;
        public string YourName => _networker.YourName;
        public long Latency => _networker.Latency;
        public Optional<bool> Successful => _networker.Successful;

        private Messenger(INetworker networker)
        {
            _networker = networker;
            _networker.ReceivedCallback = ReceivedMessage;
        }

        public static Messenger CreateClient(string url, int port, string name)
        {
            var messenger = new PeerToPeerClient();
            messenger.Init(url, port, name);
            return new Messenger(messenger);
        }
        
        public static Messenger CreateHost(int port, string name, int maxConnections = 1000)
        {
            var messenger = new PeerToPeerHost();
            messenger.Init(port, name, maxConnections);
            return new Messenger(messenger);
        }

        public void Send(object item)
        {
            if (OutOfOrderMessages.Count > 0)
                UnsentMessages.Add(item);
            else
            {
                var message = new Message(messageHistory.Count, item);
                messageHistory.Add(message);
                _networker.Send(message);
            }
        }

        private void ReceivedMessage(string json)
        {
            JObject jObj = JsonConvert.DeserializeObject<JObject>(json);
            long number = jObj.Value<long>("Number");
            Type type = Type.GetType(jObj.Value<string>("Type"));
            JToken jToken = jObj.GetValue("Value");
            var message = new Message(number, jToken.ToObject(type));

            if (message.Number == messageHistory.Count)
            {
                World.Publish(message.Value);
                messageHistory.Add(message);
                HandleExistingOutOfOrderMessages();
                if (OutOfOrderMessages.Count == 0)
                    SendUnsentMessages();
            }
            else if(message.Number > messageHistory.Count)
                OutOfOrderMessages.Add(message);
        }

        private void SendUnsentMessages()
        {
            while (UnsentMessages.Count > 0)
            {
                var unsentMessage = new Message(messageHistory.Count, UnsentMessages[0]);
                UnsentMessages.RemoveAt(0);
                _networker.Send(unsentMessage);
                messageHistory.Add(unsentMessage);
            }
        }

        private void HandleExistingOutOfOrderMessages()
        {
            while (OutOfOrderMessages.Exists((m) => m.Number == messageHistory.Count))
            {
                var oldMessage = OutOfOrderMessages.Find((m) => m.Number == messageHistory.Count);
                OutOfOrderMessages.Remove(oldMessage);
                World.Publish(oldMessage.Value);
                messageHistory.Add(oldMessage);
            }
        }

        public void Dispose()
        {
            _networker.Dispose();
        }

        public void StartGetIPAddress()
        {
            _ip.StartGetIPAddress();
        }

        public async Task<string> GetIPAddress()
        {
            return await _ip.GetIPAddress();
        }
    }
}
