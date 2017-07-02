using System.Collections.Generic;
using System.Linq;
using GoldenArrow.Game;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class InGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            return UIFactory.CreatePlayerResourceBar(new Vector2(100, 100), new PlayerResources(1))
                .Concat(UIFactory.CreatePlayerResourceBar(new Vector2(100, 200), new PlayerResources(2)))
                .Concat(UIFactory.CreatePlayerResourceBar(new Vector2(100, 300), new PlayerResources(3)))
                .Concat(UIFactory.CreatePlayerResourceBar(new Vector2(100, 400), new PlayerResources(4)));
        }
    }
}
