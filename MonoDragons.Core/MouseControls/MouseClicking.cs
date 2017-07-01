﻿using System;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public sealed class MouseClicking : ISystem
    {
        private MouseSnapshot _mouse = new MouseSnapshot();

        public void Update(IEntities entities, TimeSpan delta)
        {
            _mouse = _mouse.Current();

            if (!_mouse.ButtonJustPressed)
                return;

            entities.With<MouseClickListener>(m => m.OnClick(_mouse.Position));
            entities.With<MouseClickTarget>((o, m) => o.Transform
                .If(x => x.Intersects(_mouse.Position), 
                    () => m.OnHit(), 
                    () => m.OnMiss()));
        }
    }
}