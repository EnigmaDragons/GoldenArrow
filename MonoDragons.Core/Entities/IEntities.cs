using System;

namespace MonoDragons.Core.Entities
{
    public interface IEntities
    {
        void ForEach(Action<GameObject> action);
        void With<T>(Action<GameObject, T> action);
    }
}
