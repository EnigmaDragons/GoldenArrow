using System;

namespace MonoDragons.Core.MouseControls
{
    public sealed class HoverAction
    {
        public Action OnEnter { get; set; }
        public Action OnExit { get; set; }

        public HoverAction(Action onEnter, Action onExit)
        {
            OnEnter = onEnter;
            OnExit = onExit;
        }
    }
}
