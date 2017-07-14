using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;
using System;

namespace GoldenArrow.Scenes
{
    public class TimTestScene : IScene
    {
        private string image;
        private Transform2 transform;

        public void Init()
        {
            image = "Images/Cards/stone";
            transform = new Transform2(new Rectangle(500, 500, 272, 370));
        }

        public void Draw()
        {
            World.Draw(image, transform);
        }

        public void Update(TimeSpan delta)
        {
            transform.Rotation = new Rotation2(transform.Rotation.Value + 1f);
        }
    }
}
