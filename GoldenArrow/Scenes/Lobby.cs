using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;

namespace GoldenArrow.Scenes
{
    public class Lobby : EcsScene
    {
        private Messenger _messenger;

        public Lobby(Messenger messenger)
        {
            _messenger = messenger;
            _messenger.StartGetIPAddress();
        }

        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(700, 200), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => _messenger.CachedIPAddress.HasValue ? _messenger.CachedIPAddress.Value : "Insert Waiting for internet message here"
                });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 300), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => !_messenger.Successful.HasValue ? "Connecting ..."
                        : _messenger.Successful.Value ? (_messenger.ConnectionsCount + 1) + " out of 4 players connected" : "Failed to Connect"
                });

            // Clean this up
            yield return Entity
                .Create(new Transform2(new Vector2(300, 400), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => _messenger.YourName
                });
            yield return Entity
                .Create(new Transform2(new Vector2(500, 400), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => _messenger.ConnectionsCount > 0 ? _messenger.ConnectionNames[0] : ""
                });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 400), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => _messenger.ConnectionsCount > 1 ? _messenger.ConnectionNames[1] : ""
                });
            yield return Entity
                .Create(new Transform2(new Vector2(900, 400), new Size2(200, 50)))
                .Add(new TextDisplay
                {
                    Text = () => _messenger.ConnectionsCount > 2 ? _messenger.ConnectionNames[2] : ""
                });

            yield return UIFactory.CreateButton(new Vector2(700, 500), "Leave", () => { _messenger.Dispose(); NavigateToScene("Setup"); });
        }
    }
}
