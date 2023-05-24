using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class SelectionBoxBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldUpdateSelectionBounds()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<SelectionBoxBehavior>();
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            var selectionBoxBehavior = diagram.GetBehavior<SelectionBoxBehavior>()!;
            bool boundsChangedEventInvoked = false;
            Rectangle? lastBounds = null;
            selectionBoxBehavior.SelectionBoundsChanged += (_, newBounds) =>
            {
                boundsChangedEventInvoked = true;
                lastBounds = newBounds;
            };

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(200, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.True(boundsChangedEventInvoked);
            Assert.Equal(100, lastBounds!.Width);
            Assert.Equal(50, lastBounds.Height);
            Assert.Equal(100, lastBounds.Top);
            Assert.Equal(100, lastBounds.Left);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotUpdateSelectionBounds()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = null;
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            var selectionBoxBehavior = diagram.GetBehavior<SelectionBoxBehavior>()!;
            bool boundsChangedEventInvoked = false;
            Rectangle? lastBounds = null;
            selectionBoxBehavior.SelectionBoundsChanged += (_, newBounds) =>
            {
                boundsChangedEventInvoked = true;
                lastBounds = newBounds;
            };

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(200, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.False(boundsChangedEventInvoked);
            Assert.Null(lastBounds);
        }

        [Fact]
        public void Behavior_WithBoundsChangedDelegate_ShouldSelectNodesInsideArea()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<SelectionBoxBehavior>();
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            var selectionBoxBehavior = diagram.GetBehavior<SelectionBoxBehavior>()!;
            selectionBoxBehavior.SelectionBoundsChanged += (_, _) => { };

            var node = new NodeModel()
            {
                Size = new Size(100, 100),
                Position = new Point(150, 150)
            };
            diagram.Nodes.Add(node);

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(300, 300, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.True(node.Selected);

            diagram.TriggerPointerMove(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            Assert.False(node.Selected);
        }

        [Fact]
        public void Behavior_WithoutBoundsChangedDelegate_ShouldNotSelectNodesInsideArea()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<SelectionBoxBehavior>();
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            var node = new NodeModel()
            {
                Size = new Size(100, 100),
                Position = new Point(150, 150)
            };
            diagram.Nodes.Add(node);

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(300, 300, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.False(node.Selected);

            diagram.TriggerPointerMove(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            Assert.False(node.Selected);
        }
    }
}
