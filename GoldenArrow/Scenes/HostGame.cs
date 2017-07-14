using System.Collections.Generic;
using GoldenArrow.Events;
using GoldenArrow.NetworkEvents;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
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
            var name = UIFactory.CreateTextInput(new Vector2(650, 500), 300, "Name");
            yield return name;
            yield return UIFactory.CreateButton(new Vector2(700, 600), "Go", () =>
            {
                var connection = PeerToPeerHost.CreateConnected(int.Parse(port.Get<TypingInput>().Value), 3,
                    x => Messenger.SendMessage(new PlayerConnected { Name = name.Get<TypingInput>().Value, Id = x.UniqueIdentifier }),
                    () => World.Publish(new ConnectionFailed()));
                connection.OnDisconnect = x => Messenger.SendMessage(new PlayerDisconnected { });
                new Messenger(connection);
                var myIp = new MyIP();
                myIp.StartGetIPAddress();
                NavigateToScene(new Lobby(myIp, port.Get<TypingInput>().Value, name.Get<TypingInput>().Value));
            });
        }
    }
}
