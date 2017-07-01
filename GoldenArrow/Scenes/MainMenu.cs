using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Audio;
using MonoDragons.Core.Entities;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.PhysicsEngine;
using MonoDragons.Core.Scenes;

namespace GoldenArrow.Scenes
{
    public class MainMenu : EcsScene
    {
        protected override IEnumerable<GameObject> CreateObjs()
        {
            yield return Entity
               .Create(new Transform2(new Size2(1600, 900)))
               .Add(new Sprite("Images/Menu/", "mainmenu"))
               .Add(new BackgroundMusic("Music/maintheme"));
            yield return Entity
                .Create(new Transform2(new Vector2(300, 90), new Size2(1000, 171)))
                .Add(new Sprite("Images/Menu/", "maintitle"));
            yield return Entity
                .Create(new Transform2(new Vector2(700, 650), new Size2(200, 90)))
                .Add(new Sprite("Images/Menu/", "startgame"))
                .Add(x => new MouseStateActions
                {
                    OnReleased = () => NavigateToScene("Setup"),
                    OnHover = () => x.With<Sprite>(s => s.Name = "startgame-hover"),
                    OnPressed = () => x.With<Sprite>(s => s.Name = "startgame-pressed"),
                    OnExit = () => x.With<Sprite>(s => s.Name = "startgame"),
                });
        }
    }
}
