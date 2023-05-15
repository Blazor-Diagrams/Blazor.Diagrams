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
    public class DiagramBehaviourOptionsTests
    {
        [Fact]
        public void DiagramBehaviorOptions_DragBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramDragBehavior = DiagramDragBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));

            diagram.Options.Behaviors.DiagramDragBehavior = DiagramDragBehavior.Pan;
            Assert.True(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltDragBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramAltDragBehavior = DiagramDragBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));

            diagram.Options.Behaviors.DiagramAltDragBehavior = DiagramDragBehavior.Pan;
            Assert.True(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlDragBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramCtrlDragBehavior = DiagramDragBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));

            diagram.Options.Behaviors.DiagramCtrlDragBehavior = DiagramDragBehavior.Pan;
            Assert.True(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftDragBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramShiftDragBehavior = DiagramDragBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));

            diagram.Options.Behaviors.DiagramShiftDragBehavior = DiagramDragBehavior.Pan;
            Assert.True(diagram.IsBehaviorEnabled(new PointerEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0, 0, 0, string.Empty, true), DiagramDragBehavior.Pan));
        }

        [Fact]
        public void DiagramBehaviorOptions_DefaultScrollBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramWheelBehavior = DiagramWheelBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));

            diagram.Options.Behaviors.DiagramWheelBehavior = DiagramWheelBehavior.Zoom;
            Assert.True(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));
        }

        [Fact]
        public void DiagramBehaviorOptions_AltScrollBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramAltWheelBehavior = DiagramWheelBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));

            diagram.Options.Behaviors.DiagramAltWheelBehavior = DiagramWheelBehavior.Zoom;
            Assert.True(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, false, true, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));
        }

        [Fact]
        public void DiagramBehaviorOptions_CtrlScrollBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramCtrlWheelBehavior = DiagramWheelBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));

            diagram.Options.Behaviors.DiagramCtrlWheelBehavior = DiagramWheelBehavior.Zoom;
            Assert.True(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, true, false, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));
        }

        [Fact]
        public void DiagramBehaviorOptions_ShiftScrollBehavior_IsEnabled()
        {
            var diagram = new TestDiagram();
            diagram.Options.Behaviors.DiagramShiftWheelBehavior = DiagramWheelBehavior.None;
            Assert.False(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));

            diagram.Options.Behaviors.DiagramShiftWheelBehavior = DiagramWheelBehavior.Zoom;
            Assert.True(diagram.IsBehaviorEnabled(new WheelEventArgs(0, 0, 0, 0, false, true, false, 0, 0, 0, 0), DiagramWheelBehavior.Zoom));
        }
    }
}
