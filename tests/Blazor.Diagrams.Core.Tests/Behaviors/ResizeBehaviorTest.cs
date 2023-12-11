using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
	public class ResizeBehaviorTest
	{
		[Fact]
		public void ShouldRecalculateSizeAndPosition_TopLeft()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.TopLeft);

			// before resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);


			// after resize
			node.Position.X.Should().Be(10);
			node.Position.Y.Should().Be(15);
			node.Size.Width.Should().Be(90);
			node.Size.Height.Should().Be(185);
		}

		[Fact]
		public void ShouldRecalculateSizeAndPosition_TopRight()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.TopRight);

			// before resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// after resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(15);
			node.Size.Width.Should().Be(110);
			node.Size.Height.Should().Be(185);
		}

		[Fact]
		public void ShouldRecalculateSizeAndPosition_BottomLeft()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.BottomLeft);

			// before resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// after resize
			node.Position.X.Should().Be(10);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(90);
			node.Size.Height.Should().Be(215);
		}

		[Fact]
		public void ShouldRecalculateSizeAndPosition_BottomRight()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.BottomRight);

			// before resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// after resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(110);
			node.Size.Height.Should().Be(215);
		}

		[Fact]
		public void ShouldStopResizingSmallerThanMinimumSize()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.TopLeft);

			// before resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(300, 300, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// after resize
			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(node.MinimumDimensions.Width);
			node.Size.Height.Should().Be(node.MinimumDimensions.Height);
		}

		[Fact]
		public void ShouldStopResizeOnPointerUp()
		{
			// setup
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.TopLeft);

			// resize
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// after resize
			node.Position.X.Should().Be(10);
			node.Position.Y.Should().Be(15);
			node.Size.Width.Should().Be(90);
			node.Size.Height.Should().Be(185);

			diagram.TriggerPointerUp(null, eventArgs);

			// move pointer after pointer up
			eventArgs = new PointerEventArgs(30, 50, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			// should be no change
			node.Position.X.Should().Be(10);
			node.Position.Y.Should().Be(15);
			node.Size.Width.Should().Be(90);
			node.Size.Height.Should().Be(185);
		}

		[Fact]
		public void NodeUpdatesWhenScrollingWhileResizing()
		{
			// Arrange
			var diagram = new TestDiagram();
			diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
			var node = new NodeModel(position: new Point(0, 0));
			node.Size = new Size(100, 200);
			var resizer = node.AddResizer(ResizerPosition.TopLeft);

			node.Position.X.Should().Be(0);
			node.Position.Y.Should().Be(0);
			node.Size.Width.Should().Be(100);
			node.Size.Height.Should().Be(200);

			// Act
			var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerDown(resizer, eventArgs);
			eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
			diagram.TriggerPointerMove(null, eventArgs);

			diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ScrollBehavior>();
			diagram.Options.Zoom.ScaleFactor = 1.05;
			node.Size = new Size(100, 200);
			diagram.Nodes.Add(node);

			diagram.SelectModel(node, false);
			diagram.TriggerPointerDown(node,
			new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
			diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 200, 0, 0));

			// Assert
			node.Position.X.Should().Be(10);
			node.Position.Y.Should().Be(-175);
			node.Size.Width.Should().Be(90);
			node.Size.Height.Should().Be(185);
		}
	}
}
