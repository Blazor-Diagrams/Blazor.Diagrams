using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class ZoomBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldZoom()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ZoomBehavior>();
            diagram.SetContainer(new Rectangle(0, 0, 100, 100));

            // Act
            diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 0, 100, 0, 0));

            // Assert
            Assert.Equal(1.05, diagram.Zoom);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotZoom()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.BehaviorOptions.DiagramWheelBehavior = null;
            diagram.SetContainer(new Rectangle(0, 0, 100, 100));

            // Act
            diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 0, 100, 0, 0));

            // Assert
            Assert.Equal(1, diagram.Zoom);
        }
    }
}
