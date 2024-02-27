using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Bunit;
using Xunit;


namespace Blazor.Diagrams.Core.Tests.Models
{
    public class NodeModelTest
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

            using var ctx = new TestContext();

            //Arrange
            var diagram = new TestDiagram();
            diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
            var node = new NodeModel(position: new Point(100, 100));
            node.Size = new Size(100, 100);

            var port = new PortModel(node, alignment);
            node.AddPort(port);

            var cut = ctx.RenderComponent<NodeWidget>(parameters => parameters
                       .Add(n => n.Node, node).AddCascadingValue(diagram));

            var newX = 200;
            var newY = 300;

            //Act
            node.SetPosition(newX, newY);

            //Assert
            Assert.Equal(200, port.Position.X);
            Assert.Equal(300, port.Position.Y);
        }

        [Theory]
        [InlineData(PortAlignment.Top, 300, 100)]
        [InlineData(PortAlignment.TopLeft, 100, 100)]
        [InlineData(PortAlignment.TopRight, 500, 100)]
        [InlineData(PortAlignment.Bottom, 300, 700)]
        [InlineData(PortAlignment.BottomLeft, 100, 700)]
        [InlineData(PortAlignment.BottomRight, 500, 700)]
        [InlineData(PortAlignment.Left, 100, 400)]
        [InlineData(PortAlignment.Right, 500, 400)]
        public void UpdatePortOnSetSize(PortAlignment alignment, double expectedX, double expectedY)
        {
            // Arrange
            var oldWidth = 100.0;
            var oldHeight = 100.0;
            var newWidth = 500.0;
            var newHeight = 700.0;
            var node = new NodeModel(new Point(100, 100)) { Size = new Size(oldWidth, oldHeight) };
            var port = node.AddPort(alignment);

            // Act
            node.SetSize(newWidth, newHeight);

            // Assert
            Assert.Equal(expectedX, port.Position.X);
            Assert.Equal(expectedY, port.Position.Y);
        }
    }
}
