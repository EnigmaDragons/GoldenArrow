using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class HostGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return UIFactory.CreateTextInput(new Vector2(750, 400), 100, "Port");
            yield return UIFactory.CreateButton(new Vector2(700, 500), "Go", () => NavigateToScene("Lobby"));
        }
    }
}
