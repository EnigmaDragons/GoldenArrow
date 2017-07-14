using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoDragons.Core.Networking
{
    public class Messenger : IDisposable
    {
        public static Messenger AppMessenger;

        private IMessenger _networker;
        private List<Message> messageHistory = new List<Message>();
        private List<Message> OutOfOrderMessages = new List<Message>();
        private List<object> UnsentMessages = new List<object>();
        public Optional<bool> ConnectionSuccessful => _networker.ConnectionSuccessful;
        public long UniqueIdentifier => _networker.UniqueIdentifier;

        public Messenger(IMessenger networker)
        {
            _networker = networker;
            _networker.ReceivedCallback = ReceivedMessage;
            AppMessenger = this;
        }

        public void Send(object item)
        {
            if (OutOfOrderMessages.Count > 0)
                UnsentMessages.Add(item);
            else
            {
                var message = new Message(messageHistory.Count, item);
                messageHistory.Add(message);
                _networker.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }

        public void SendHistory()
        {
            if (OutOfOrderMessages.Count == 0 && UnsentMessages.Count == 0)
                foreach (var message in messageHistory)
                    _networker.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        private void ReceivedMessage(byte[] bytesAsJson)
        {
            JObject jObj = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(bytesAsJson));
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
                _networker.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unsentMessage)));
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
    }
}
