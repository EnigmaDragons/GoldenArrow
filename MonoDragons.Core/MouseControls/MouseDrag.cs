using Microsoft.Xna.Framework;
using MonoDragons.Core.Common;

namespace MonoDragons.Core.MouseControls
{
    public sealed class MouseDrag
    {
        public bool IsBeingDragged { get; set; }
        public Point LastDragPoint { get; set; }
        public Point DragPoint { get; set; }
        public Optional<Point> DropPoint { get; set; }

        public void UpdateDragPoint(Point location)
        {
            LastDragPoint = DragPoint;
            DragPoint = location;
        }

        public void Drop(Point location)
        {
            IsBeingDragged = false;
            DragPoint = Point.Zero;
            DropPoint = location;
        }
    }
}
