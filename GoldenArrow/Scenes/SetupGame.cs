using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class SetupGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(700, 400), new Size2(200, 70)))
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new TextDisplay { Text = () => "Create" })
                .Add(x => new MouseStateActions
                {
                    OnReleased = () => { },
                    OnHover = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Red).Create()),
                    OnPressed = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Pink).Create()),
                    OnExit = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.DarkRed).Create()),
                });

            yield return Entity
                .Create(new Transform2(new Vector2(700, 500), new Size2(200, 70)))
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new TextDisplay { Text = () => "Join" })
                .Add(x => new MouseStateActions
                {
                    OnReleased = () => {},
                    OnHover = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Red).Create()),
                    OnPressed = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Pink).Create()),
                    OnExit = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.DarkRed).Create()),
                });
        }

        private GameObject CreateGameModal()
        {
            
        }
    }
}
