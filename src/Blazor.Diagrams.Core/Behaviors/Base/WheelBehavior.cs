using Blazor.Diagrams.Core.Events;

namespace Blazor.Diagrams.Core.Behaviors.Base
{
    public abstract class WheelBehavior : Behavior
    {
        protected WheelBehavior(Diagram diagram)
            : base(diagram)
        {

            Diagram.Wheel += OnDiagramWheel;
        }

        protected abstract void OnDiagramWheel(WheelEventArgs e);

        public virtual bool IsBehaviorEnabled(WheelEventArgs e)
        {
            if (e.AltKey && !e.CtrlKey && !e.ShiftKey
                && Diagram.BehaviorOptions.DiagramAltWheelBehavior is not null)
            {
                return this == Diagram.BehaviorOptions.DiagramAltWheelBehavior;
            }
            else if (!e.AltKey && e.CtrlKey && !e.ShiftKey
                && Diagram.BehaviorOptions.DiagramCtrlWheelBehavior is not null)
            {
                return this == Diagram.BehaviorOptions.DiagramCtrlWheelBehavior;
            }
            else if (!e.AltKey && !e.CtrlKey && e.ShiftKey
                && Diagram.BehaviorOptions.DiagramShiftWheelBehavior is not null)
            {
                return this == Diagram.BehaviorOptions.DiagramShiftWheelBehavior;
            }

            return this == Diagram.BehaviorOptions.DiagramWheelBehavior;
        }

        public override void Dispose()
        {
            Diagram.Wheel -= OnDiagramWheel;
        }
    }
}
