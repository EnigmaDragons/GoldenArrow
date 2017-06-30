using System;

namespace MonoDragons.Core.Entities
{
    public static class EntitiesExtensions
    {
        public static void With<T>(this IEntities entities, Action<T> action)
        {
            entities.With<T>((o, t) => action(t));
        }
    }
}
