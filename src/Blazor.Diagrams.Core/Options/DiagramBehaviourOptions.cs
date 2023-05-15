using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Options
{
    public class DiagramBehaviorOptions
    {
        public DiagramWheelBehavior DiagramWheelBehavior { get; set; } = DiagramWheelBehavior.Zoom;

        public DiagramWheelBehavior DiagramAltWheelBehavior { get; set; } = DiagramWheelBehavior.Zoom;

        public DiagramWheelBehavior DiagramCtrlWheelBehavior { get; set; } = DiagramWheelBehavior.Zoom;

        public DiagramWheelBehavior DiagramShiftWheelBehavior { get; set; } = DiagramWheelBehavior.Zoom;

        public DiagramDragBehavior DiagramDragBehavior { get; set; } = DiagramDragBehavior.Pan;

        public DiagramDragBehavior DiagramAltDragBehavior { get; set; } = DiagramDragBehavior.Pan;

        public DiagramDragBehavior DiagramCtrlDragBehavior { get; set; } = DiagramDragBehavior.Pan;

        public DiagramDragBehavior DiagramShiftDragBehavior { get; set; } = DiagramDragBehavior.Select;
    }

    public enum DiagramWheelBehavior
    {
        None,
        Zoom,
        Scroll,
    }

    public enum DiagramDragBehavior
    {
        None,
        Pan,
        Select,
    }
}
