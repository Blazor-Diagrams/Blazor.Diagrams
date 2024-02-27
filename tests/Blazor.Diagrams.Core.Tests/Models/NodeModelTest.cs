using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Moq;
using Xunit;


namespace Blazor.Diagrams.Core.Tests.Models
{
    public class NodeModelTest
    {
        [Fact]
        public void UpdatePortOnSetPosition()
        {
            var node = new NodeModel(position: new Point(100, 100));
            node.Size = new Size(100, 100);

            var port = new PortModel(node, PortAlignment.BottomLeft, new Point(50, 50));
            node.AddPort(port);


            var newX = 200;
            var newY = 300;

            //Act
            node.SetPosition(newX, newY);

            //Assert
            Assert.Equal(150, port.Position.X);
            Assert.Equal(250, port.Position.Y);
        }

        [Fact]
        public void SetPortPositionOnNodeSizeChangedIsCalledOnSetSize()
        {
            // Arrange
            var oldWidth = 100.0;
            var oldHeight = 100.0;
            var newWidth = 500.0;
            var newHeight = 700.0;
            var deltaX = newWidth - oldWidth;
            var deltaY = newHeight - oldHeight;

            var node = new NodeModel(new Point(100, 100)) { Size = new Size(oldWidth, oldHeight) };
            var portMock = new Mock<PortModel>(node, PortAlignment.BottomLeft, null, null);

            node.AddPort(portMock.Object);

            // Act
            node.SetSize(newWidth, newHeight);

            // Assert
            portMock.Verify(m => m.SetPortPositionOnNodeSizeChanged(deltaX, deltaY), Times.Once);
        }
    }
}
