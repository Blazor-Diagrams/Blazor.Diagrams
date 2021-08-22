using Blazor.Diagrams.Core.Geometry;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests
{
    public class DiagramTests
    {
        [Fact]
        public void GetScreenPoint_ShouldReturnCorrectPoint()
        {
            // Arrange
            var diagram = new Diagram();

            // Act
            diagram.SetZoom(1.234);
            diagram.UpdatePan(50, 50);
            diagram.SetContainer(new Rectangle(30, 65, 1000, 793));
            var pt = diagram.GetScreenPoint(100, 200);

            // Assert
            pt.X.Should().Be(203.4); // 2*X + panX + left
            pt.Y.Should().Be(361.8); // 2*Y + panY + top
        }
    }
}
