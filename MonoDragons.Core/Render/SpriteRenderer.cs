using System.Linq;
using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;

namespace MonoDragons.Core.Render
{
    public sealed class SpriteRenderer : IRenderer
    {
        public void Draw(IEntities entities)
        {
            entities.Collect<Sprite>()
                .OrderByDescending(x => x.Transform.ZIndex)
                    .ForEach(t => t.With<Sprite>(s => World.Draw(s.Name, t.Transform)));
        }
    }
}
