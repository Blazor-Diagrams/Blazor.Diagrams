using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Options;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class ScrollBehavior : Behavior
    {
        public ScrollBehavior(Diagram diagram)
            : base(diagram)
        {
            Diagram.Wheel += Diagram_Wheel;
        }

        void Diagram_Wheel(WheelEventArgs e)
        {
            if (Diagram.Container == null || !Diagram.IsBehaviorEnabled(e, DiagramWheelBehavior.Scroll))
                return;

            var x = Diagram.Pan.X + (e.DeltaX / Diagram.Options.Zoom.ScaleFactor);
            var y = Diagram.Pan.Y + (e.DeltaY / Diagram.Options.Zoom.ScaleFactor);

            Diagram.SetPan(x, y);
        }

        public override void Dispose()
        {
            Diagram.Wheel -= Diagram_Wheel;
        }
    }
}
