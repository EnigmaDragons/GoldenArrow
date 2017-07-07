using System;
using System.Collections.Generic;
using System.Linq;
using GoldenArrow.Game;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class InGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            return CreatePlayerResourceBar(new Vector2(100, 100), new PlayerState(1))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 200), new PlayerState(2)))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 300), new PlayerState(3)))
                .Concat(CreatePlayerResourceBar(new Vector2(100, 400), new PlayerState(4)));
        }

        private static List<GameObject> CreatePlayerResourceBar(Vector2 position, PlayerState state)
        {
            var x = position.X;
            var y = position.Y;

            var resourceData = new Map<string, Func<string>>
            {
                ["wood"] = () => state.Wood.ToString(),
                ["food"] = () => state.Food.ToString(),
                ["gold"] = () => state.Gold.ToString(),
                ["stone"] = () => state.Stone.ToString(),
                ["population"] = () => state.Population.ToString(),
                ["happiness"] = () => state.Happiness.ToString()
            };

            var resourceIcons = new List<GameObject>();
            foreach (KeyValuePair<string, Func<string>> entry in resourceData)
            {
                resourceIcons.Add(CreateResourceIcon(new Vector2(x, y), entry.Key, entry.Value));
                x += IconSize + 5;
            }
            return resourceIcons;

        }

        private static GameObject CreateResourceIcon(Vector2 position, string iconName, Func<string> value)
        {
            return Entity
                .Create(new Transform2(position, new Size2(IconSize, IconSize)))
                .Add(new Sprite("Images/Icons/", iconName))
                .Add(new TextDisplay { Text = value });
        }

        private const int IconSize = 40;
    }
}
