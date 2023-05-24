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
            diagram.Options.Behaviors.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.Options.Behaviors.DiagramAltDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramAltDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.Options.Behaviors.DiagramCtrlDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramCtrlDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftDragBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.Options.Behaviors.DiagramShiftDragBehavior = null;
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramDragBehavior = null;
            Assert.False(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));

            diagram.Options.Behaviors.DiagramShiftDragBehavior = diagram.GetBehavior<PanBehavior>();
            Assert.True(diagram.GetBehavior<PanBehavior>()!.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true)));
        }

        [Fact]
        public void DiagramBehaviorOptions_DefaultScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.Options.Behaviors.DiagramAltWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramAltWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.Options.Behaviors.DiagramCtrlWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramCtrlWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0)));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftScrollBehavior_IsBehaviorEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.Options.Behaviors.DiagramShiftWheelBehavior = null;
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramWheelBehavior = null;
            Assert.False(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));

            diagram.Options.Behaviors.DiagramShiftWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            Assert.True(diagram.GetBehavior<ZoomBehavior>()!.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0)));
        }
    }
}
