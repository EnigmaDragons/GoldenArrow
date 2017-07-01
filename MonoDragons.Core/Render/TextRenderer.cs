using MonoDragons.Core.Entities;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Render
{
    public sealed class TextRenderer : IRenderer
    {
        public void Draw(IEntities entities)
        {
            entities.With<TextDisplay>(
                (o, t) => UI.DrawTextAligned(t.Text(), o.Transform.WithPadding(t.Margin).ToRectangle(), t.Color, t.Font, t.Align));
        }
    }
}
