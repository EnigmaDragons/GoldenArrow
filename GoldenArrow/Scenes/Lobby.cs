using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class Lobby : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(700, 200), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => "Host IP: Not Implemented"
                });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 300), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => "Not Implemented out of 4 players connected"
                });
            yield return UIFactory.CreateButton(new Vector2(700, 500), "Leave", () => NavigateToScene("Setup"));
        }
    }
}
