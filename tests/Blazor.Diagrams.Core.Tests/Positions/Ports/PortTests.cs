using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Positions.Ports
{
    public class PortTests
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
        public void UpdatePortOnSetPosition(PortAlignment alignment)
        {
            //Arrange
            var diagram = new TestDiagram();
            diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
            var node = new NodeModel(position: new Point(100, 100));
            node.Size = new Size(100, 100);

            var port = node.AddPort(alignment);

            var newX = 200;
            var newY = 300;

            //Act
            node.SetPosition(newX, newY);

            //Assert
            Assert.Equal(200, port.Position.X);
            Assert.Equal(300, port.Position.Y);
        }


        [Theory]
        [InlineData(PortAlignment.Top)]
        [InlineData(PortAlignment.TopLeft)]
        [InlineData(PortAlignment.TopRight)]
        [InlineData(PortAlignment.Bottom)]
        [InlineData(PortAlignment.BottomLeft)]
        [InlineData(PortAlignment.BottomRight)]
        [InlineData(PortAlignment.Left)]
        [InlineData(PortAlignment.Right)]
        public void UpdatePortOnSetSize(PortAlignment alignment)
        {
            // Arrange
            var oldWidth = 100.0;
            var oldHeight = 100.0;
            var newWidth = 500.0;
            var newHeight = 700.0;
            var node = new NodeModel(position: new Point(100, 100)) { Size = new Size(oldWidth, oldHeight) };
            var port = node.AddPort(alignment);

            var deltaX = newWidth - oldWidth;
            var deltaY = newHeight - oldHeight;

            Point expected = alignment switch
            {
                PortAlignment.Top => new Point(port.Position.X + deltaX / 2, port.Position.Y),
                PortAlignment.TopRight => new Point(port.Position.X + deltaX, port.Position.Y),
                PortAlignment.TopLeft => new Point(port.Position.X, port.Position.Y),
                PortAlignment.Right => new Point(port.Position.X + deltaX, port.Position.Y + deltaY / 2),
                PortAlignment.Left => new Point(port.Position.X, port.Position.Y + deltaY / 2),
                PortAlignment.Bottom => new Point(port.Position.X + deltaX / 2, port.Position.Y + deltaY),
                PortAlignment.BottomRight => new Point(port.Position.X + deltaX, port.Position.Y + deltaY),
                PortAlignment.BottomLeft => new Point(port.Position.X, port.Position.Y + deltaY),
                _ => new Point(port.Position.X, port.Position.Y)
            };

            // Act
            node.SetSize(newWidth, newHeight);

            // Assert
            Assert.Equal(expected.X, port.Position.X);
            Assert.Equal(expected.Y, port.Position.Y);
        }


    }
}
