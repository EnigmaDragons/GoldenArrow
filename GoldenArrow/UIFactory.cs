using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;

namespace GoldenArrow
{
    public static class UIFactory
    {
        public static GameObject CreateButton(Vector2 position, string text, Action onClick)
        {
            return Entity
                .Create(new Transform2(position, new Size2(200, 70)))
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new TextDisplay { Text = () => text })
                .Add(x => new MouseStateActions
                {
                    OnReleased = onClick,
                    OnHover = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Red).Create()),
                    OnPressed = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Pink).Create()),
                    OnExit = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.DarkRed).Create()),
                });
        }
    }
}
