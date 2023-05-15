using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Options;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class PanBehaviorTests
    {
        [Fact]
        public void Behavior_WhenBehaviorEnabled_ShouldPan()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<PointerEventArgs>(), It.IsAny<DiagramDragBehavior>())).Returns((PointerEventArgs _, DiagramDragBehavior behaviour) => behaviour == DiagramDragBehavior.Pan);
            diagram.Object.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            Assert.Equal(0, diagram.Object.Pan.X);
            Assert.Equal(0, diagram.Object.Pan.Y);

            // Act
            diagram.Object.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.Object.TriggerPointerMove(null,
                new PointerEventArgs(200, 200, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.Equal(100, diagram.Object.Pan.X);
            Assert.Equal(100, diagram.Object.Pan.Y);
        }

        [Fact]
        public void Behavior_WhenBehaviorDisabled_ShouldNotPan()
        {
            // Arrange
            var diagram = new Mock<TestDiagram>(null) { CallBase = true };
            diagram.Setup(d => d.IsBehaviorEnabled(It.IsAny<PointerEventArgs>(), It.IsAny<DiagramDragBehavior>())).Returns(false);
            diagram.Object.SetContainer(new Rectangle(Point.Zero, new Size(100, 100)));

            Assert.Equal(0, diagram.Object.Pan.X);
            Assert.Equal(0, diagram.Object.Pan.Y);

            // Act
            diagram.Object.TriggerPointerDown(null,
                new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
            diagram.Object.TriggerPointerMove(null,
                new PointerEventArgs(200, 200, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

            // Assert
            Assert.Equal(0, diagram.Object.Pan.X);
            Assert.Equal(0, diagram.Object.Pan.Y);
        }
    }
}
