using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Memory;

namespace MonoDragons.Core.Render
{
    public sealed class BorderRenderer : IRenderer
    {
        public void Draw(IEntities entities, SpriteBatch sprites)
        {
            entities.With<BorderTexture>((o, b) => {
                Resources.Put(b.Value.GetHashCode().ToString(), b.Value);
                sprites.Draw(b.Value, o.Transform.ToRectangle(), Color.White);
            });
        }
    }
}
