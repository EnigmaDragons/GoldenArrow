using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.Common;
using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public class MouseHovering : ISystem
    {
        private readonly List<GameObject> _targets = new List<GameObject>();

        public void Update(IEntities entities, TimeSpan delta)
        {
            var pos = Mouse.GetState().Position;

            UpdateHoverExits(pos);
            UpdateHoverTarget(entities, pos);
        }

        private void UpdateHoverExits(Point mouse)
        {
            _targets.Copy()
                .ForEach(x => x.Transform.If(t => !t.Intersects(mouse), t =>
                {
                    x.With<HoverAction>(h => h.OnExit());
                    _targets.Remove(x);
                }));
        }

        private void UpdateHoverTarget(IEntities entities, Point pos)
        {
            if (_targets.Any())
                return;

            var possibleTargets = new List<GameObject>();
            entities.ForEach(e => e.With<HoverAction>(x => e.Transform.If(t => t.Intersects(pos), t => possibleTargets.Add(e))));
            if (possibleTargets.Any())
                _targets.Add(possibleTargets.OrderByDescending(x => x.Transform.ZIndex).First());
            _targets.ForEach(x => x.With<HoverAction>(e => e.OnEnter()));
        }
    }
}
