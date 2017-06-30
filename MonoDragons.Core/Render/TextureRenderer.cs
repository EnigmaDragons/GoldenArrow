using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.Render
{
    public class TextureRenderer : IRenderer
    {
        public void Draw(IEntities entities)
        {
            entities.With<Texture>((o, t) =>
                World.Draw(t.Value, o.Transform));
        }
    }
}
