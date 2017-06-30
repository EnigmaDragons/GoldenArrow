using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class MainMenu : IScene
    {
        private readonly List<GameObject> _objs = new List<GameObject>();

        public void Create()
        {
            _objs.Add(Entity
                .Create(new Transform2(new Size2(1600, 900)))
                .Add(new Sprite("Images/Menu/mainmenu")));
            _objs.Add(Entity
                .Create(new Transform2(new Vector2(300, 90), new Size2(1000, 171)))
                .Add(new Sprite("Images/Menu/maintitle")));
            _objs.Add(Entity
                .Create(new Transform2(new Vector2(700, 650), new Size2(200, 90)))
                .Add(new Sprite("Images/Menu/startgame"))
                .Add(new ClickAction(StartGame)));
        }

        private void StartGame()
        {
            Entity.Destroy(_objs);
            World.NavigateToScene("Table");
        }


        public void Init()
        {
            Create();
        }

        public void Update(TimeSpan delta)
        {
        }

        public void Draw()
        {
        }
    }
}
