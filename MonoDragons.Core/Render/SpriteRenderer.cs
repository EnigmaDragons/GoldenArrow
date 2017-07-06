using System.Linq;
using MonoDragons.Core.Common;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Memory;

namespace MonoDragons.Core.Render
{
    public sealed class SpriteRenderer : IRenderer
    {
        public void Draw(IEntities entities, SpriteBatch sprites)
        {
            entities.Collect<Sprite>()
                .OrderByDescending(x => x.Transform.ZIndex)
                    .ForEach(t => t.With<Sprite>(s =>
                        sprites.Draw(Resources.Load<Texture2D>(s.Name), t.Transform.ToRectangle(), Color.White)));
        }
    }
}
