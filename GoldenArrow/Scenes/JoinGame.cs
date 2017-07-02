using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;
using MonoDragons.Core.KeyboardControls;

namespace GoldenArrow.Scenes
{
    public class JoinGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            var ipAddress = UIFactory.CreateTextInput(new Vector2(650, 300), 300, "IP Address");
            yield return ipAddress;
            var port = UIFactory.CreateTextInput(new Vector2(750, 400), 100, "Port");
            yield return port;
            yield return UIFactory.CreateButton(new Vector2(550, 500), "Cancel", () => NavigateToScene("Setup"));
            yield return UIFactory.CreateButton(new Vector2(850, 500), "Go",
                () => { Messenger.CreateClient(ipAddress.Get<TypingInput>().Value, int.Parse(port.Get<TypingInput>().Value));
                NavigateToScene("Lobby"); });
        }
    }
}
