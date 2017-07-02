using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class SetupGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return UIFactory.CreateButton(new Vector2(700, 400), "Create", () => NavigateToScene("Host"));
            yield return UIFactory.CreateButton(new Vector2(700, 500), "Join", () => NavigateToScene("Join"));
        }
    }
}
