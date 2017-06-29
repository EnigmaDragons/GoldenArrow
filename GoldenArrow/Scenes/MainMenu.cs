using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Entities;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class MainMenu : IScene
    {
        public static void Create()
        {
            var background = Entity
                .Create(new Transform2(new Size2(1600, 900)))
                .Add(new Sprite("Images/Menu/mainmenu"));
            var title = Entity
                .Create(new Transform2(new Vector2(300, 90), new Size2(1000, 171)))
                .Add(new Sprite("Images/Menu/maintitle"));
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
