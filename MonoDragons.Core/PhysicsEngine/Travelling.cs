using System;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.PhysicsEngine
{
    public sealed class Travelling : ISystem
    {
        public void Update(IEntities entities, TimeSpan delta)
        {
            entities.With<DurationTravel>(
                (o, x) => o.Transform.Location = x.GetNewPosition(o.Transform.Location, delta));
        }
    }
}
