using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class ScrollBehavior : WheelBehavior
    {
        public ScrollBehavior(Diagram diagram)
            : base(diagram)
        {
        }

        protected override void OnDiagramWheel(WheelEventArgs e)
        {
            if (Diagram.Container == null || !IsBehaviorEnabled(e))
                return;

            Diagram.UpdatePan(-e.DeltaX / Diagram.Zoom, -e.DeltaY / Diagram.Zoom);
        }
    }
}
