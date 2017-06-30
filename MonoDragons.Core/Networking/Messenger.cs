using MonoDragons.Core.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.Core.Networking
{
    public class Messenger
    {
        private INetworker _messenger;
        private List<Message> messageHistory = new List<Message>();
        private List<Message> OutOfOrderMessages = new List<Message>();
        private List<object> UnsentMessages = new List<object>();

        private Messenger(INetworker networker)
        {
            _messenger = networker;
            _messenger.ReceivedCallback = ReceivedMessage;
        }

        public static Messenger CreateClient(string url, int port)
        {
            var messenger = new PeerToPeerClient();
            messenger.Init(url, port);
            return new Messenger(messenger);
        }
        
        public static Messenger CreateHost(int port)
        {
            var messenger = new PeerToPeerHost();
            messenger.Init(port);
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
                _messenger.Send(message);
            }
        }

        public void ReceivedMessage(string json)
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
                while (OutOfOrderMessages.Exists((m) => m.Number == messageHistory.Count))
                {
                    var oldMessage = OutOfOrderMessages.Find((m) => m.Number == messageHistory.Count);
                    OutOfOrderMessages.Remove(oldMessage);
                    World.Publish(oldMessage.Value);
                    messageHistory.Add(oldMessage);
                }
                if(OutOfOrderMessages.Count == 0)
                    while (UnsentMessages.Count > 0)
                    {
                        var unsentMessage = new Message(messageHistory.Count, UnsentMessages[0]);
                        UnsentMessages.RemoveAt(0);
                        _messenger.Send(unsentMessage);
                        messageHistory.Add(unsentMessage);
                    }
            }
            else if(message.Number > messageHistory.Count)
                OutOfOrderMessages.Add(message);
        }
    }
}
