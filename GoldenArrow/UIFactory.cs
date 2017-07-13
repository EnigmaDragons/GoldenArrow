using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GoldenArrow.Game;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.KeyboardControls;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Text;
using MonoGame.Cards.Cards;

namespace GoldenArrow
{
    public static class UIFactory
    {
        public static GameObject CreateButton(Vector2 position, string text, Action onClick)
        {
            return Entity
                .Create(new Transform2(position, new Size2(200, 70)))
                .Add(new Texture { Value = new RectangleTexture(200, 70, Color.DarkRed).Create() })
                .Add(new TextDisplay
                {
                    Text = () => text,
                    Font = "Fonts/12-bold",
                })
                .Add(x => new MouseStateActions
                {
                    OnReleased = onClick,
                    OnHover = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Red).Create()),
                    OnPressed = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.Pink).Create()),
                    OnExit = () => x.With<Texture>(s => s.Value = new RectangleTexture(200, 70, Color.DarkRed).Create()),
                });
        }

        public static GameObject CreateTextInput(Vector2 position, int width, string watermark)
        {
            return Entity
                .Create(new Transform2(position, new Size2(width, 50)))
                .Add(new TypingInput { IsActive = false })
                .Add(new Texture { Value = new RectangleTexture(width, 50, Color.White).Create() })
                .Add(x => new TextDisplay
                {
                    Color = Color.FromNonPremultiplied(0, 0, 0, 100),
                    Align = TextAlign.Left,
                    Text = () =>
                    {
                        bool isActive = false;
                        string textInputValue = null;
                        x.With<TypingInput>(y =>
                        {
                            isActive = y.IsActive;
                            textInputValue = y.Value;
                        });
                        return !isActive && string.IsNullOrWhiteSpace(textInputValue) 
                            ? watermark 
                            : textInputValue;
                    },
                })
                .Add(x => new MouseClickTarget
                {
                    OnHit = () => 
                    {
                        x.With<TypingInput>(y => y.IsActive = true);
                        x.With<TextDisplay>(y => y.Color = Color.Black);
                    },
                    OnMiss = () =>
                    {
                        x.With<TextDisplay>(y =>
                        {
                            if (string.IsNullOrWhiteSpace(y.Text()))
                                y.Color = Color.FromNonPremultiplied(0, 0, 0, 100);
                        });
                        x.With<TypingInput>(y => y.IsActive = false);
                    }
                });
        }

        public static GameObject CreateCard(Card card)
        {
            return Entity.Create(new Transform2(CardSize()))
                .Add(card)
                .Add(card.Sprite)
                .Add(new MouseDrag());
        }

        public static Size2 CardSize()
        {
            return new Size2(256, 354);
        }
    }
}
