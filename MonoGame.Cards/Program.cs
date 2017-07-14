using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Audio;
using MonoDragons.Core.EngimaDragons;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Entities;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.MouseControls;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoGame.Cards.Decks;
using MonoGame.Cards.Scenes;

namespace MonoGame.Cards
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            MouseSystems.RegisterAll(Entity.System);
            SoundSystems.RegisterAll(Entity.System);
            using (var game = new NeedlesslyComplexMainGame("MonoGame.Cards", "Table", new Display(1600, 900, false, 1), CreateSceneFactory(), CreateController()))
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
                });
        }
    }
#endif
}
