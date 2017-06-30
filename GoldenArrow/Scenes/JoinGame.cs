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
                .Create(new Transform2(new Vector2()));
            yield return Entity
                .Create(new Transform2(new Size2(300, 100)))
                .Add(new TypingInput { IsActive = true })
                .Add(x => new TextDisplay { Text = () => x.Get<TypingInput>().Value });
        }
    }
}
