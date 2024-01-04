using Blazor.Diagrams.Core.Behaviors.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Options;

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

            var x = Diagram.Pan.X - (e.DeltaX / Diagram.Options.Zoom.ScaleFactor);
            var y = Diagram.Pan.Y - (e.DeltaY / Diagram.Options.Zoom.ScaleFactor);
            Diagram.GetScreenPoint(x, y);

            var _lastClientX = e.ClientX - e.DeltaX;
            var _lastClientY = e.ClientY - e.DeltaY;

            var deltaX = e.ClientX - _lastClientX + (Diagram.Pan.X - x);
            var deltaY = e.ClientY - _lastClientY + (Diagram.Pan.Y - y);

            Diagram.SetPan(x, y, deltaX, deltaY, _lastClientX, _lastClientY);
        }
    }
}
