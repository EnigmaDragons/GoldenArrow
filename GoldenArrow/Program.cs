using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.EngimaDragons;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Inputs;
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
            using (var game = new NeedlesslyComplexMainGame("Golden Arrow", "TimTest", new Display(1600, 900, false, 1), CreateSceneFactory(), CreateController()))
                game.Run();
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
                    { "Table", () => new Table() },
                    { "TimTest", () => new TimTestScene() }
                });
        }
    }
#endif
}
