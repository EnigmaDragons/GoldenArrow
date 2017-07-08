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
        public Optional<string> CachedIPAddress = new Optional<string>();
        public bool IsFull { get { return _networker.IsFull; } }
        public int ConnectionsCount { get { return _networker.ConnectionsCount; } }
        public long Latency { get { return _networker.Latency; } }
        public Optional<bool> Successful => _networker.Successful;

        private Messenger(INetworker networker)
        {
            _networker = networker;
            _networker.ReceivedCallback = ReceivedMessage;
        }

        public static Messenger CreateClient(string url, int port)
        {
            var messenger = new PeerToPeerClient();
            messenger.Init(url, port);
            return new Messenger(messenger);
        }
        
        public static Messenger CreateHost(int port, int maxConnections = 1000)
        {
            var messenger = new PeerToPeerHost();
            messenger.Init(port, maxConnections);
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
                        _networker.Send(unsentMessage);
                        messageHistory.Add(unsentMessage);
                    }
            }
            else if(message.Number > messageHistory.Count)
                OutOfOrderMessages.Add(message);
        }

        public void Dispose()
        {
            _networker.Dispose();
        }

        public async void StartGetIPAddress()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://myexternalip.com/raw");
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                var stream = response.GetResponseStream();
                var x = Encoding.UTF8.GetString(ExtractResponse(response.ContentLength, response.GetResponseStream()));
                CachedIPAddress = new Optional<string>(x.Substring(0, x.IndexOf("\n")));
            }
        }

        public async Task<string> GetIPAddress()
        {
            if (CachedIPAddress.HasValue)
                return CachedIPAddress.Value;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://myexternalip.com/raw");
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                var stream = response.GetResponseStream();
                CachedIPAddress = new Optional<string>(Encoding.UTF8.GetString(ExtractResponse(response.ContentLength, response.GetResponseStream())));
                CachedIPAddress = new Optional<string>(CachedIPAddress.Value.Substring(0, CachedIPAddress.Value.IndexOf("/n")));
                return CachedIPAddress.Value;
            }
        }

        private byte[] ExtractResponse(long length, Stream stream)
        {
            byte[] data;
            using (var mstrm = new MemoryStream())
            {
                var tempBuffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(tempBuffer, 0, tempBuffer.Length)) != 0)
                    mstrm.Write(tempBuffer, 0, bytesRead);
                mstrm.Flush();
                data = mstrm.GetBuffer();
            }
            return data;
        }
    }
}
