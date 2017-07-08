using MonoDragons.Core.Common;
using System;

namespace MonoDragons.Core.Networking
{
    public interface INetworker : IDisposable
    {
        Action<string> ReceivedCallback { get; set; }
        void Send(object item);
        int ConnectionsCount { get; }
        bool IsFull { get; }
        long Latency { get; }
        Optional<bool> Successful { get; }
    }
}