using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class JoinGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return UIFactory.CreateTextInput(new Vector2(650, 300), 300, "IP Address");
            yield return UIFactory.CreateTextInput(new Vector2(750, 400), 100, "Port");
            yield return UIFactory.CreateButton(new Vector2(550, 500), "Cancel", () => NavigateToScene("Setup"));
            yield return UIFactory.CreateButton(new Vector2(850, 500), "Go", () => NavigateToScene("Lobby"));
        }
    }
}
