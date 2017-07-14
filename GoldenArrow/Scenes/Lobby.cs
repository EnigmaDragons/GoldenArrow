using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;
using MonoDragons.Core.Graphics;

namespace GoldenArrow.Scenes
{
    public class Lobby : EcsScene
    {
        private Messenger _messenger;
        private MyIP myIP;

        public Lobby(Messenger messenger)
        {
            _messenger = messenger;
            myIP = new MyIP();
            myIP.StartGetIPAddress();
        }

        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(700, 200), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => myIP.CachedIPAddress.HasValue ? myIP.CachedIPAddress.Value : "Insert Waiting for internet message here"
                });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 300), new Size2(200, 50)));

            yield return Entity
                .Create(new Transform2(new Vector2(0, 0), new Size2(100, 100)))
                .Add(new Texture() { Value = new RectangleTexture(new Rectangle(0,0,1,1),
                    _messenger.ConnectionSuccessful.HasValue ? _messenger.ConnectionSuccessful.Value ? Color.Green : Color.Red : Color.Yellow)
                    .Create() });

            yield return UIFactory.CreateButton(new Vector2(700, 500), "Leave", () => { _messenger.Dispose(); NavigateToScene("Setup"); });
        }
    }
}
