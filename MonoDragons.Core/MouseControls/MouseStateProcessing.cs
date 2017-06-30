using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public sealed class MouseStateProcessing : ISystem
    {
        private Microsoft.Xna.Framework.Input.MouseState _state;
        private Microsoft.Xna.Framework.Input.MouseState _lastState;

        // @todo #1 Add support for other Mouse Button Clicks
        private bool LeftIsPressed => _state.LeftButton == ButtonState.Pressed;
        private bool MouseUp => _state.LeftButton == ButtonState.Released;
        private bool LeftMouseButtonJustPressed => _lastState.LeftButton != ButtonState.Pressed && LeftIsPressed;
        public bool LeftMouseButtonJustReleased => _lastState.LeftButton == ButtonState.Pressed && !LeftIsPressed;
        
        public void Update(IEntities entities, TimeSpan delta)
        {
            _state = Mouse.GetState();
            var pos = _state.Position;

            entities.With<MouseStateActions>((o, m) =>
            {
                if (!o.Transform.Intersects(pos))
                    m.Exit();
                else if (!o.Transform.Intersects(_lastState.Position))
                    m.Hover();
                else if (LeftMouseButtonJustPressed)
                    m.Click();
                else if (LeftMouseButtonJustReleased)
                    m.Release();
            });

            // TODO: Kill this
            if (LeftMouseButtonJustPressed)
                entities.With<MouseDownAction>((o, t) => o.Transform.If(x => x.Intersects(pos), () => t.Action()));

            _lastState = _state;
        }
    }
}
