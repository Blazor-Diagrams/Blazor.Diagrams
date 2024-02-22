using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models
{
    public class PortModelTest
    {
        [Theory]
        [InlineData(PortAlignment.Top)]
        [InlineData(PortAlignment.TopLeft)]
        [InlineData(PortAlignment.TopRight)]
        [InlineData(PortAlignment.Bottom)]
        [InlineData(PortAlignment.BottomLeft)]
        [InlineData(PortAlignment.BottomRight)]
        [InlineData(PortAlignment.Left)]
        [InlineData(PortAlignment.Right)]
        public void SetPortPositionOnNodeSizeChangedCalculatesCorrectPosition(PortAlignment alignment)
        {
            // Arrange
            var node = new NodeModel();
            var port = new PortModel(node, alignment, new Point(0, 0));
            node.Size = new Size(100, 100);

            // Act
            port.SetPortPositionOnNodeSizeChanged(100, 100);

            // Assert
            switch (alignment)
            {
                case PortAlignment.Top:
                    Assert.Equal(50, port.Position.X);
                    Assert.Equal(0, port.Position.Y);
                    break;
                case PortAlignment.TopRight:
                    Assert.Equal(100, port.Position.X);
                    Assert.Equal(0, port.Position.Y);
                    break;
                case PortAlignment.TopLeft:
                    Assert.Equal(0, port.Position.X);
                    Assert.Equal(0, port.Position.Y);
                    break;
                case PortAlignment.Right:
                    Assert.Equal(100, port.Position.X);
                    Assert.Equal(50, port.Position.Y);
                    break;
                case PortAlignment.Left:
                    Assert.Equal(0, port.Position.X);
                    Assert.Equal(50, port.Position.Y);
                    break;
                case PortAlignment.Bottom:
                    Assert.Equal(50, port.Position.X);
                    Assert.Equal(100, port.Position.Y);
                    break;
                case PortAlignment.BottomRight:
                    Assert.Equal(100, port.Position.X);
                    Assert.Equal(100, port.Position.Y);
                    break;
                case PortAlignment.BottomLeft:
                    Assert.Equal(0, port.Position.X);
                    Assert.Equal(100, port.Position.Y);
                    break;
            }
        }
    }
}
