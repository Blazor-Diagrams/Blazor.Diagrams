using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class PanBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldPan()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = diagram.GetBehavior<PanBehavior>();
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            Assert.Equal(0, diagram.Pan.X);
            Assert.Equal(0, diagram.Pan.Y);

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(200, 200, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.Equal(100, diagram.Pan.X);
            Assert.Equal(100, diagram.Pan.Y);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotPan()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramDragBehavior = null;
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            Assert.Equal(0, diagram.Pan.X);
            Assert.Equal(0, diagram.Pan.Y);

            // Act
            diagram.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.TriggerPointerMove(null,
                new PointerEventArgs(200, 200, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.Equal(0, diagram.Pan.X);
            Assert.Equal(0, diagram.Pan.Y);
        }
    }
}
