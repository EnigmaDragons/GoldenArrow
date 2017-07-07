using System.Collections.Generic;
using System.Linq;
using GoldenArrow.Game;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public sealed class GameSummary : EcsScene
    {
        private const int IconWidth = 64;
        private const int PlayerWidth = 100;
        private const int Padding = 20;
        private static readonly Size2 IconSize = new Size2(IconWidth, IconWidth);

        protected override IEnumerable<GameObject> CreateObjs()
        {
            var players = CreatePlayers();
            var width = IconWidth + players.Count * (PlayerWidth + Padding) + (Padding * 2);
            var leftEdge = (1600 - width) / (float) 2;
            yield return Entity.Create(new Transform2(new Vector2(leftEdge, 100), new Size2(width, 700)))
                .Add(new Texture {Value = new RectangleTexture(400, 700, Color.DarkRed).Create()});
            yield return Entity.Create(new Transform2(new Vector2(leftEdge, 100), new Size2(width, 100)))
                .Add(new TextDisplay { Text = () => "Game Summary" });
            yield return CreateDividingLine(new Transform2(new Vector2(leftEdge + 50, 200), new Size2(width - 100, 3)));
            yield return CreateDividingLine(new Transform2(new Vector2(leftEdge + 50, 350), new Size2(width - 100, 3)));
            yield return CreateIcon(new Vector2(leftEdge + Padding, 400), "happiness");
            yield return CreateIcon(new Vector2(leftEdge + Padding, 500), "population");
            yield return CreateIcon(new Vector2(leftEdge + Padding, 600), "building");
            yield return CreateDividingLine(new Transform2(new Vector2(leftEdge + 50, 690), new Size2(width - 100, 3)));

            var playerX = leftEdge + Padding * 2 + IconWidth;
            foreach (var player in players)
            {
                var state = player.Get<PlayerState>();
                yield return CreatePlayerLabel(new Vector2(playerX, 250), "P" + state.Player);
                yield return CreatePointLabel(new Vector2(playerX, 400), state.Happiness);
                yield return CreatePointLabel(new Vector2(playerX, 500), state.Population);
                yield return CreatePointLabel(new Vector2(playerX, 600), state.PointsFromBuildings);
                yield return CreatePointLabel(new Vector2(playerX, 700), state.TotalVictoryPoints);
                yield return player;
                playerX += PlayerWidth + Padding;
            }
        }

        private GameObject CreateDividingLine(Transform2 transform)
        {
            return Entity.Create(transform)
                .Add(new Texture {Value = new RectangleTexture(new Size2(1, 1), Color.White).Create()});
        }

        private List<GameObject> CreatePlayers()
        {
            var players = new List<GameObject>();
            for (var i = 0; i < 4; i++)
                players.Add(Entity.Create(Transform2.Zero)
                    .Add(x => new PlayerState(x.Id)));
            return players;
        }

        private GameObject CreateIcon(Vector2 position, string name)
        {
            return Entity.Create(new Transform2(position, IconSize))
                .Add(new Sprite("Images/Icons/", name));
        }

        private GameObject CreatePlayerLabel(Vector2 position, string name)
        {
            return Entity.Create(new Transform2(position, IconSize))
                .Add(new TextDisplay { Text = () => name });
        }

        private GameObject CreatePointLabel(Vector2 position, int points)
        {
            return Entity.Create(new Transform2(position, IconSize))
                .Add(new TextDisplay { Text = () => points.ToString()});
        }
    }
}
