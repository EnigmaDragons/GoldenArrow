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
                .ForEach(t => t.With<Sprite>(s =>
                    sprites.Draw(Resources.Load<Texture2D>(s.Name), null, t.Transform.ToRectangle(), null, null,
                        t.Transform.Rotation.Value * .017453292519f, new Vector2(1, 1), null, SpriteEffects.None, 
                            t.Transform.ZIndex / (float)int.MaxValue)));
        }
    }
}
