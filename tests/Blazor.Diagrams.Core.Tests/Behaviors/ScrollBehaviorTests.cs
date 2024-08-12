using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class ScrollBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldScroll()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ScrollBehavior>();
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));
            diagram.Options.Zoom.ScaleFactor = 1.05;

            // Act
            diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 200, 0, 0));

            // Assert
            Assert.Equal(-95, diagram.Pan.X, 0);
            Assert.Equal(-190, diagram.Pan.Y, 0);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotScroll()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            diagram.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            // Act
            diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 200, 0, 0));

            // Assert
            Assert.Equal(0, diagram.Pan.X);
            Assert.Equal(0, diagram.Pan.Y);
        }
    }
}
