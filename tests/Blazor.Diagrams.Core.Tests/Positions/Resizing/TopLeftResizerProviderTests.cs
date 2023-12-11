using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Positions.Resizing;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Positions.Resizing
{
    public class TopLeftResizerProviderTests
    {
        [Fact]
        public void DragResizer_ShouldResizeNode()
        {
            // setup
            var diagram = new TestDiagram();
            diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
            var node = new NodeModel(position: new Point(0, 0));
            node.Size = new Size(100, 200);
            var control = new ResizeControl(new TopLeftResizerProvider());
            diagram.Controls.AddFor(node).Add(control);
            diagram.SelectModel(node, false);

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            control.OnPointerDown(diagram, node, eventArgs);
            eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(10);
            node.Position.Y.Should().Be(15);
            node.Size.Width.Should().Be(90);
            node.Size.Height.Should().Be(185);
        }

        [Fact]
        public void DragResizer_SmallerThanMinSize_SetsNodeToMinSize()
        {
            // setup
            var diagram = new TestDiagram();
            diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
            var node = new NodeModel(position: new Point(0, 0));
            node.Size = new Size(100, 200);
            var control = new ResizeControl(new TopLeftResizerProvider());
            diagram.Controls.AddFor(node).Add(control);
            diagram.SelectModel(node, false);

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            control.OnPointerDown(diagram, node, eventArgs);
            eventArgs = new PointerEventArgs(99, 199, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);
            eventArgs = new PointerEventArgs(300, 300, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(99);
            node.Position.Y.Should().Be(199);
            node.Size.Width.Should().Be(0);
            node.Size.Height.Should().Be(0);
        }
    }
}
