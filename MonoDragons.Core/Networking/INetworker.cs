using System;
using System.Collections.Generic;

namespace MonoDragons.Core.Networking
{
    public interface INetworker : IDisposable
    {
        Action<string> ReceivedCallback { get; set; }
        void Send(object item);
    }
}