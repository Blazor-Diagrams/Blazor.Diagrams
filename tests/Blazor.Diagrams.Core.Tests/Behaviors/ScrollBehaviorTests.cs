using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Options;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class ScrollBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldScroll()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<WheelEventArgs>(), It.IsAny<DiagramWheelBehavior>()))
                .Returns((WheelEventArgs _, DiagramWheelBehavior behaviour) => behaviour == DiagramWheelBehavior.Scroll);
            diagram.Object.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));
            diagram.Object.Options.Zoom.ScaleFactor = 1.05;

            // Act
            diagram.Object.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 200, 0, 0));

            // Assert
            Assert.Equal(-95, diagram.Object.Pan.X, 0);
            Assert.Equal(-190, diagram.Object.Pan.Y, 0);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotScroll()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<WheelEventArgs>(), It.IsAny<DiagramWheelBehavior>()))
                .Returns(false);
            diagram.Object.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            // Act
            diagram.Object.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 200, 0, 0));

            // Assert
            Assert.Equal(0, diagram.Object.Pan.X);
            Assert.Equal(0, diagram.Object.Pan.Y);
        }
    }
}
