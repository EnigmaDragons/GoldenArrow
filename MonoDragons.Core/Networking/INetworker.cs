using MonoDragons.Core.Common;
using System;
using System.Collections.Generic;

namespace MonoDragons.Core.Networking
{
    public interface INetworker : IDisposable
    {
        Action<string> ReceivedCallback { get; set; }
        void Send(object item);
        string YourName { get; }
        List<string> ConnectionNames { get; }
        int ConnectionsCount { get; }
        bool IsFull { get; }
        long Latency { get; }
        Optional<bool> Successful { get; }
    }
}