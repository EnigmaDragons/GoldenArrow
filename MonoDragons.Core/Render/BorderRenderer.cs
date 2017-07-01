using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.Render
{
    public sealed class BorderRenderer : IRenderer
    {
        public void Draw(IEntities entities)
        {
            entities.With<BorderTexture>((o, b) => World.Draw(b.Value, o.Transform));
        }
    }
}
