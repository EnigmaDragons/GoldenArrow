using MonoDragons.Core.Entities;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Render
{
    public sealed class TextRenderer : IRenderer
    {
        public void Draw(IEntities entities)
        {
            entities.With<TextDisplay>(
                    (o, text) => UI.DrawTextCentered(text.Text(), o.Transform.ToRectangle(), text.Color, text.Font));
        }
    }
}
