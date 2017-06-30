﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public sealed class MouseStateProcessing : ISystem
    {
        private Microsoft.Xna.Framework.Input.MouseState _state;
        private Point _lastPos;
        private bool _leftWasPressed;

        // @todo #1 Add support for other Mouse Button Clicks
        private bool LeftIsPressed => _state.LeftButton == ButtonState.Pressed;
        private bool MouseUp => _state.LeftButton == ButtonState.Released;
        private bool LeftMouseButtonJustPressed => !_leftWasPressed && LeftIsPressed;
        public bool LeftMouseButtonJustReleased => _leftWasPressed && !LeftIsPressed;
        
        public void Update(IEntities entities, TimeSpan delta)
        {
            _state = Mouse.GetState();
            var pos = _state.Position;

            entities.With<MouseStateActions>((o, m) =>
            {
                if (!o.Transform.Intersects(pos))
                    m.Exit();
                else if (!o.Transform.Intersects(_lastPos))
                    m.Hover();
                else if (LeftMouseButtonJustPressed)
                    m.Click();
                else if (LeftMouseButtonJustReleased)
                    m.Release();
            });

            // TODO: Kill this
            if (LeftMouseButtonJustPressed)
                entities.ForEach(e => e.With<MouseDownAction>(t => e.Transform.If(x => x.Intersects(pos), () => t.Action())));

            _leftWasPressed = LeftIsPressed;
            _lastPos = pos;
        }
    }
}