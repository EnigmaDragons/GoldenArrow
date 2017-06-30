using MonoDragons.Core.Entities;

namespace MonoDragons.Core.MouseControls
{
    public static class MouseSystems
    {
        public static void RegisterAll(EntitySystem system)
        {
            system.Register(new MouseClicking());
            system.Register(new MouseDragging());
            system.Register(new MouseHovering());
        }
    }
}
