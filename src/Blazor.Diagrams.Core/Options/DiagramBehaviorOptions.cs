using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Behaviors.Base;
using System;
using System.Runtime.CompilerServices;

namespace Blazor.Diagrams.Core.Options
{
    public class DiagramBehaviorOptions
    {
        public WheelBehavior? DiagramWheelBehavior { get; set; }

        public WheelBehavior? DiagramAltWheelBehavior { get; set; }

        public WheelBehavior? DiagramCtrlWheelBehavior { get; set; }

        public WheelBehavior? DiagramShiftWheelBehavior { get; set; }

        public DragBehavior? DiagramDragBehavior { get; set; }

        public DragBehavior? DiagramAltDragBehavior { get; set; }

        public DragBehavior? DiagramCtrlDragBehavior { get; set; }

        public DragBehavior? DiagramShiftDragBehavior { get; set; }
    }
}
