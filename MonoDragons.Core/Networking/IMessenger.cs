using System;
using System.Collections.Generic;

namespace MonoDragons.Core.Networking
{
    public interface IMessenger
    {
        void Send(object item);
        List<string> GetNewMessages { get; }
        void Update(TimeSpan delta);
    }
}