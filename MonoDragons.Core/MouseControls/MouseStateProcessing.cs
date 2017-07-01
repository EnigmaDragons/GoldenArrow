using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public sealed class MouseStateProcessing : ISystem
    {
        private MouseSnapshot _mouse = new MouseSnapshot();
        
        public void Update(IEntities entities, TimeSpan delta)
        {
            _mouse = _mouse.Current();

            entities.With<MouseStateActions>((o, m) =>
            {
                if (!o.Transform.Intersects(_mouse.Position))
                    m.Exit();
                else if (!o.Transform.Intersects(_mouse.LastPosition))
                    m.Hover();
                else if (_mouse.ButtonJustPressed)
                    m.Click();
                else if (_mouse.ButtonJustReleased)
                    m.Release();
            });
        }
    }
}
