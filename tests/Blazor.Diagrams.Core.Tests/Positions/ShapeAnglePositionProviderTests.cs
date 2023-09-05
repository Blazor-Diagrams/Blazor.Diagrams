using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Positions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Positions;

public class ShapeAnglePositionProviderTests
{
    [Fact]
    public void GetPosition_ShouldUseGetPointAtAngleOfShape()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var provider = new ShapeAnglePositionProvider(70);
        nodeMock.Setup(n => n.GetShape()).Returns(shapeMock.Object);

        // Act
        var position = provider.GetPosition(nodeMock.Object);

        // Assert
        shapeMock.Verify(m => m.GetPointAtAngle(70), Times.Once);
    }

    [Fact]
    public void GetPosition_ShouldUseOffset_WhenProvided()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var provider = new ShapeAnglePositionProvider(70, 5, -10);
        nodeMock.Setup(n => n.GetShape()).Returns(shapeMock.Object);
        shapeMock.Setup(s => s.GetPointAtAngle(70)).Returns(new Point(100, 50));

        // Act
        var position = provider.GetPosition(nodeMock.Object);

        // Assert
        position!.X.Should().Be(105);
        position.Y.Should().Be(40);
    }
}