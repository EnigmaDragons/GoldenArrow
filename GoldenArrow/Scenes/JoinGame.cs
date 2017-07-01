using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.KeyboardControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class JoinGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(600, 300), new Size2(100, 100)))
                .Add(new TextDisplay { Text = () => "IP: " });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 300), new Size2(300, 100)))
                .Add(new TypingInput { IsActive = true })
                .Add(x => new TextDisplay { Text = () => x.Get<TypingInput>().Value })
            yield return Entity
                .Create(new Transform2(new Vector2(600, 400), new Size2(100, 100)))
                .Add(new TextDisplay { Text = () => "Port: " });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 400), new Size2(300, 100)))
                .Add(new TypingInput { IsActive = false })
                .Add(x => new TextDisplay { Text = () => x.Get<TypingInput>().Value });
            yield return UIFactory.CreateButton(new Vector2(550, 500), "Cancel", () => NavigateToScene("Setup"));
            yield return UIFactory.CreateButton(new Vector2(850, 500), "Go", () => { });
        }
    }
}
