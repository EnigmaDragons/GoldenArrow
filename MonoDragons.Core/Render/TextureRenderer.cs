using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Memory;

namespace MonoDragons.Core.Render
{
    public class TextureRenderer : IRenderer
    {
        public void Draw(IEntities entities, SpriteBatch sprites)
        {
            entities.With<Texture>((o, t) => {
                Resources.Put(t.Value.GetHashCode().ToString(), t.Value);
                sprites.Draw(t.Value, o.Transform.ToRectangle(), Color.White);
            });
        }
    }
}
