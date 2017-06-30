using System;

namespace MonoDragons.Core.MouseControls
{
    public class MouseStateActions
    {
        public MouseState CurrentState { get; set; } = MouseState.None;
        public DateTime ClickedAt { get; set; } = DateTime.MinValue;
        
        public Action OnReleased { get; set; } = () => {};
        public Action OnHover { get; set; } = () => {};
        public Action OnPressed { get; set; } = () => {};
        public Action OnExit { get; set; } = () => {};

        public void Hover()
        {
            OnHover();
            CurrentState = MouseState.Hovered;
        }

        public void Exit()
        {
            if(CurrentState != MouseState.None)
                OnExit();
            CurrentState = MouseState.None;
        }

        public void Click()
        {
            ClickedAt = DateTime.Now;
            OnPressed();
            CurrentState = MouseState.Pressed;
        }

        public void Release()
        {
            OnHover();
            if ((DateTime.Now - ClickedAt).Milliseconds < 150)
                OnReleased();
            CurrentState = MouseState.None;
        }
    }
}
