using System;
using System.Linq;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.PhysicsEngine
{
    public sealed class ZGravitation : ISystem
    {
        public void Update(IEntities entities, TimeSpan delta)
        {
            var layered = entities.Collect<ZGravity>().OrderBy(x => x.Transform.ZIndex);
            GameObject lastObj;
            foreach (var obj in layered)
            {
                
            }

        }
    }
}
