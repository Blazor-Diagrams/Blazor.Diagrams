using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Options;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class ZoomBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldZoom()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<WheelEventArgs>(), It.IsAny<DiagramWheelBehavior>()))
                .Returns((WheelEventArgs _, DiagramWheelBehavior behaviour) => behaviour == DiagramWheelBehavior.Zoom);
            diagram.Object.SetContainer(new Rectangle(0, 0, 100, 100));

            // Act
            diagram.Object.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 0, 100, 0, 0));

            // Assert
            Assert.Equal(1.05, diagram.Object.Zoom);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotZoom()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<WheelEventArgs>(), It.IsAny<DiagramWheelBehavior>()))
                .Returns(false);
            diagram.Object.SetContainer(new Rectangle(0, 0, 100, 100));

            // Act
            diagram.Object.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 0, 100, 0, 0));

            // Assert
            Assert.Equal(1, diagram.Object.Zoom);
        }
    }
}
