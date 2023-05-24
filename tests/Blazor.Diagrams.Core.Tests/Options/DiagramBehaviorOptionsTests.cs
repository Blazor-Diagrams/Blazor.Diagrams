using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Options
{
    public class DiagramBehaviorOptionsTests
    {
        [Fact]
        public void DiagramBehaviorOptions_DragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.BehaviorOptions.DiagramAltDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramAltDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.BehaviorOptions.DiagramCtrlDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramCtrlDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.BehaviorOptions.DiagramShiftDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.BehaviorOptions.DiagramShiftDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_DefaultScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.BehaviorOptions.DiagramAltWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramAltWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.BehaviorOptions.DiagramCtrlWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramCtrlWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.BehaviorOptions.DiagramShiftWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));

            diagram.BehaviorOptions.DiagramShiftWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));
        }
    }
}
