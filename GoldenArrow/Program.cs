using System;
using System.Collections.Generic;
using GoldenArrow.Scenes;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Audio;
using MonoDragons.Core.EngimaDragons;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.KeyboardControls;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Scenes;

namespace GoldenArrow
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new NeedlesslyComplexMainGame("Golden Arrow", "MainMenu", new Display(1600, 900, false, 1), CreateSceneFactory(), CreateController()))
            {
                MouseSystems.RegisterAll(Entity.System);
                SoundSystems.RegisterAll(Entity.System);
                Entity.Register(new KeyboardInput());
                game.Run();
            }
        }

        private static IController CreateController()
        {
            return new KeyboardController(new Map<Keys, Control>
            {
                { Keys.Z, Control.A },
            });
        }

        private static SceneFactory CreateSceneFactory()
        {
            return new SceneFactory(
                new Dictionary<string, Func<IScene>>
                {
                    { "Logo", () => new LogoScene() },
                    { "MainMenu", () => new MainMenu() },
                    { "Table", () => new Table() },
                    { "Silas", () => new SilasDemoScene() },
                    { "Setup", () => new SetupGame() },
                    { "Join", () => new JoinGame() },
                    { "Host", () => new HostGame() },
                    { "InGame", () => new InGame() },
                    { "GameSummary", () => new GameSummary() },
                });
        }
    }
#endif
}
