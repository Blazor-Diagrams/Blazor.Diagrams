﻿using Blazor.Diagrams.Core.Events;

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
                && Diagram.Options.Behaviors.DiagramAltWheelBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramAltWheelBehavior;
            }
            else if (!e.AltKey && e.CtrlKey && !e.ShiftKey
                && Diagram.Options.Behaviors.DiagramCtrlWheelBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramCtrlWheelBehavior;
            }
            else if (!e.AltKey && !e.CtrlKey && e.ShiftKey
                && Diagram.Options.Behaviors.DiagramShiftWheelBehavior is not null)
            {
                return this == Diagram.Options.Behaviors.DiagramShiftWheelBehavior;
            }

            return this == Diagram.Options.Behaviors.DiagramWheelBehavior;
        }

        public override void Dispose()
        {
            Diagram.Wheel -= OnDiagramWheel;
        }
    }
}
