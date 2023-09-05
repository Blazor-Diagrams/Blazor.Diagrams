using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Anchors;

public class SinglePortAnchorTests
{
    [Fact]
    public void GetPlainPosition_ShouldReturnMiddlePosition()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50)
        };
        var anchor = new SinglePortAnchor(port);

        // Act
        var position = anchor.GetPlainPosition()!;

        // Assert
        var mp = port.MiddlePosition;
        position.X.Should().Be(mp.X);
        position.Y.Should().Be(mp.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnNull_WhenPortNotInitialized()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50)
        };
        var anchor = new SinglePortAnchor(port);
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link);

        // Assert
        position.Should().BeNull();
    }

    [Fact]
    public void GetPosition_ShouldReturnMiddlePosition_WhenMiddleIfNoMarker()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = true
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;

        // Assert
        var mp = port.MiddlePosition;
        position.X.Should().Be(mp.X);
        position.Y.Should().Be(mp.Y);
    }

    [Theory]
    [InlineData(PortAlignment.Top, 110, 50)]
    [InlineData(PortAlignment.TopRight, 120, 50)]
    [InlineData(PortAlignment.Right, 120, 55)]
    [InlineData(PortAlignment.BottomRight, 120, 60)]
    [InlineData(PortAlignment.Bottom, 110, 60)]
    [InlineData(PortAlignment.BottomLeft, 100, 60)]
    [InlineData(PortAlignment.Left, 100, 55)]
    [InlineData(PortAlignment.TopLeft, 100, 50)]
    public void GetPosition_ShouldReturnAlignmentBasedPosition_WhenUseShapeAndAlignmentIsFalse(PortAlignment alignment, double x, double y)
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent, alignment)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = false,
            UseShapeAndAlignment = false
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;

        // Assert
        position.X.Should().Be(x);
        position.Y.Should().Be(y);
    }

    [Theory]
    [InlineData(PortAlignment.Top, 270)]
    [InlineData(PortAlignment.TopRight, 315)]
    [InlineData(PortAlignment.Right, 0)]
    [InlineData(PortAlignment.BottomRight, 45)]
    [InlineData(PortAlignment.Bottom, 90)]
    [InlineData(PortAlignment.BottomLeft, 135)]
    [InlineData(PortAlignment.Left, 180)]
    [InlineData(PortAlignment.TopLeft, 225)]
    public void GetPosition_ShouldUsePointAtAngle_WhenUseShapeAndAlignmentIsTrue(PortAlignment alignment, double angle)
    {
        // Arrange
        var parent = new NodeModel();
        var shapeMock = new Mock<IShape>();
        var port = new CustomPortModel(shapeMock.Object, parent, alignment)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = false,
            UseShapeAndAlignment = true
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;
        
        // Assert
        shapeMock.Verify(s => s.GetPointAtAngle(angle), Times.Once);
    }

    private class CustomPortModel : PortModel
    {
        private readonly IShape _shape;

        public CustomPortModel(IShape shape, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(parent, alignment, position, size)
        {
            _shape = shape;
        }

        public CustomPortModel(IShape shape, string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(id, parent, alignment, position, size)
        {
            _shape = shape;
        }

        public override IShape GetShape() => _shape;
    }
}