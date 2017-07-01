using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.KeyboardControls;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class TypingScene : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(400, 400), new Size2(200, 100)))
                .Add(new TypingInput { IsActive = true })
                .Add(x => new TextDisplay {Text = () => x.Get<TypingInput>().Value})
                .Add(x => new MouseClickTarget
                {
                    OnHit = () => x.With<TypingInput>(t => t.IsActive = true), 
                    OnMiss = () => x.With<TypingInput>(t => t.IsActive = false)
                })
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new BorderTexture { Value = new RectangleBorderTexture(new Size2(200, 100), 4, 2, Color.White).Create() });

            yield return Entity
                .Create(new Transform2(new Vector2(300, 0), new Size2(200, 100)))
                .Add(new TypingInput())
                .Add(x => new TextDisplay { Text = () => x.Get<TypingInput>().Value })
                .Add(x => new MouseClickTarget
                {
                    OnHit = () => x.With<TypingInput>(t => t.IsActive = true),
                    OnMiss = () => x.With<TypingInput>(t => t.IsActive = false)
                })
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new BorderTexture { Value = new RectangleBorderTexture(new Size2(200, 100), 4, 2, Color.White).Create() });
        }
    }
}
