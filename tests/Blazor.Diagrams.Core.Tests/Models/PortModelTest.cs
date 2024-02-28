using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models
{
    public class PortModelTest
    {
        [Theory]
        [InlineData(PortAlignment.Top, 50, 0)]
        [InlineData(PortAlignment.TopLeft, 0, 0)]
        [InlineData(PortAlignment.TopRight, 100, 0)]
        [InlineData(PortAlignment.Bottom, 50, 100)]
        [InlineData(PortAlignment.BottomLeft, 0, 100)]
        [InlineData(PortAlignment.BottomRight, 100, 100)]
        [InlineData(PortAlignment.Left, 0, 50)]
        [InlineData(PortAlignment.Right, 100, 50)]
        public void SetPortPositionOnNodeSizeChangedCalculatesCorrectPosition(PortAlignment alignment, double expectedXPosition, double expectedYPosition)
        {
            // Arrange
            var node = new NodeModel();
            var port = new PortModel(node, alignment, new Point(0, 0));
            node.Size = new Size(100, 100);

            // Act
            port.SetPortPositionOnNodeSizeChanged(100, 100);

            // Assert
            Assert.Equal(expectedXPosition, port.Position.X);
            Assert.Equal(expectedYPosition, port.Position.Y);
        }
    }
}
