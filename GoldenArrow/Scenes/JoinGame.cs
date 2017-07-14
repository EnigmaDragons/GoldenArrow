using System.Collections.Generic;
using GoldenArrow.Events;
using GoldenArrow.NetworkEvents;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;
using MonoDragons.Core.KeyboardControls;
using MonoDragons.Core.Engine;

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
            var name = UIFactory.CreateTextInput(new Vector2(650, 500), 300, "Name");
            yield return name;
            yield return UIFactory.CreateButton(new Vector2(550, 600), "Cancel", () => NavigateToScene("Setup"));
            yield return UIFactory.CreateButton(new Vector2(850, 600), "Go", () =>
            {
                var connection = PeerToPeerClient.CreateConnected(ipAddress.Get<TypingInput>().Value, int.Parse(port.Get<TypingInput>().Value), 
                    x => Messenger.SendMessage(new PlayerConnected { Name = name.Get<TypingInput>().Value, Id = x.UniqueIdentifier }), 
                    () => World.Publish(new ConnectionFailed()));
                new Messenger(connection);
                NavigateToScene(new Lobby(ipAddress.Get<TypingInput>().Value, port.Get<TypingInput>().Value, name.Get<TypingInput>().Value));
            });
        }
    }
}
