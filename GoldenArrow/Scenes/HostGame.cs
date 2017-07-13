using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;
using MonoDragons.Core.KeyboardControls;

namespace GoldenArrow.Scenes
{
    public class HostGame : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            var port = UIFactory.CreateTextInput(new Vector2(750, 400), 100, "Port");
            yield return port;
            yield return UIFactory.CreateButton(new Vector2(700, 500), "Go",
                () => { var messenger = new Messenger(PeerToPeerHost.CreateConnected(int.Parse(port.Get<TypingInput>().Value), 3));
                    NavigateToScene(new Lobby(messenger)); });
        }
    }
}
