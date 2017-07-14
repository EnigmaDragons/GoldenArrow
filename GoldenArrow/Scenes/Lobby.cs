using System;
using System.Collections.Generic;
using GoldenArrow.Events;
using GoldenArrow.NetworkEvents;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Networking;
using MonoDragons.Core.Text;

namespace GoldenArrow.Scenes
{
    public class Lobby : EcsScene
    {
        private readonly Func<string> _getAddress;

        private string _players = "";

        public Lobby(MyIP ip, string port, string player) :
            this(() => ip.CachedIPAddress.HasValue ? $"Host: {ip.CachedIPAddress.Value}:{port}" : "Resolving Host Address...", player) {}

        public Lobby(string ip, string port, string player) : 
            this(() => $"Host: {ip}:{port}", player) {}

        public Lobby(Func<string> getAddress, string player)
        {
            _players = player;
            _getAddress = getAddress;
            World.Subscribe(EventSubscription.Create<ConnectionFailed>(x => Leave(), this));
            World.Subscribe(EventSubscription.Create<PlayerConnected>(x => _players = $"{_players}\n{x.Name}", this));
        }

        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
                .Create(new Transform2(new Vector2(700, 200), new Size2(200, 50)))
                .Add(new TextDisplay { Text = _getAddress });
            yield return Entity
                .Create(new Transform2(new Vector2(700, 300), new Size2(200, 50)))
                .Add(new TextDisplay { Text = () => _players, Align = TextAlign.Center });
            yield return UIFactory.CreateButton(new Vector2(700, 500), "Leave", Leave);
        }

        private void Leave()
        {
            //Messenger.AppMessenger.Dispose();
            NavigateToScene("Setup");
        } 
    }
}
