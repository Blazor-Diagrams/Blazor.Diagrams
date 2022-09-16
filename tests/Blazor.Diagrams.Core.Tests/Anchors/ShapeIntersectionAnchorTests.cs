using System.Collections.Generic;
using System.Linq;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Anchors;

public class ShapeIntersectionAnchorTests
{
    [Fact]
    public void GetPlainPosition_ShouldReturnNodeCenter()
    {
        // Arrange
        var node = new NodeModel
        {
            Size = new Size(100, 80),
            Position = new Point(60, 60)
        };
        var anchor = new ShapeIntersectionAnchor(node);

        // Act
        var position = anchor.GetPlainPosition()!;

        // Assert
        var center = node.GetBounds()!.Center;
        position.X.Should().Be(center.X);
        position.Y.Should().Be(center.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnNull_WhenNodeSizeIsNull()
    {
        // Arrange
        var node = new NodeModel(new Point(60, 60));
        var anchor = new ShapeIntersectionAnchor(node);
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link);

        // Assert
        position.Should().BeNull();
    }

    [Fact]
    public void GetPosition_ShouldUseRouteToFindOtherPositionForIntersection_WhenSource()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var args = new List<Line>();
        shapeMock.Setup(s => s.GetIntersectionsWithLine(Capture.In(args)));
        var node = new CustomNodeModel(shapeMock.Object)
        {
            Size = new Size(100, 80),
            Position = new Point(60, 60)
        };
        var source = new ShapeIntersectionAnchor(node);
        var link = new LinkModel(source, new PositionAnchor(Point.Zero));
        var route = new[] { new Point(170, 100), new Point(180, 110) };

        // Act
        source.GetPosition(link, route);

        // Assert
        var line = args.Single();
        line.Start.Should().BeEquivalentTo(route[0]);
        line.End.Should().BeEquivalentTo(node.GetBounds()!.Center);
    }

    [Fact]
    public void GetPosition_ShouldUseRouteToFindOtherPositionForIntersection_WhenTarget()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var args = new List<Line>();
        shapeMock.Setup(s => s.GetIntersectionsWithLine(Capture.In(args)));
        var node = new CustomNodeModel(shapeMock.Object)
        {
            Size = new Size(100, 80),
            Position = new Point(60, 60)
        };
        var source = new ShapeIntersectionAnchor(node);
        var target = new ShapeIntersectionAnchor(node);
        var link = new LinkModel(source, target);
        var route = new[] { new Point(170, 100), new Point(180, 110) };

        // Act
        target.GetPosition(link, route);

        // Assert
        var line = args.Single();
        line.Start.Should().BeEquivalentTo(route[^1]);
        line.End.Should().BeEquivalentTo(node.GetBounds()!.Center);
    }

    [Fact]
    public void GetPosition_ShouldCallOtherGetPlainPosition_WhenNoRoute()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var args = new List<Line>();
        shapeMock.Setup(s => s.GetIntersectionsWithLine(Capture.In(args)));
        var node = new CustomNodeModel(shapeMock.Object)
        {
            Size = new Size(100, 80),
            Position = new Point(60, 60)
        };
        var source = new ShapeIntersectionAnchor(node);
        var targetMock = new Mock<Anchor>(node);
        var pt = new Point(-55, -55);
        targetMock.Setup(t => t.GetPlainPosition()).Returns(pt);
        var link = new LinkModel(source, targetMock.Object);

        // Act
        source.GetPosition(link);

        // Assert
        var line = args.Single();
        line.Start.Should().BeEquivalentTo(pt);
        line.End.Should().BeEquivalentTo(node.GetBounds()!.Center);
    }

    [Fact]
    public void GetPosition_ShouldReturnNull_WhenOtherPositionIsNull()
    {
        // Arrange
        var shapeMock = new Mock<IShape>();
        var args = new List<Line>();
        shapeMock.Setup(s => s.GetIntersectionsWithLine(Capture.In(args)));
        var node = new CustomNodeModel(shapeMock.Object)
        {
            Size = new Size(100, 80),
            Position = new Point(60, 60)
        };
        var source = new ShapeIntersectionAnchor(node);
        var targetMock = new Mock<Anchor>(node);
        targetMock.Setup(t => t.GetPlainPosition()).Returns((Point?)null);
        var link = new LinkModel(source, targetMock.Object);

        // Act
        var position = source.GetPosition(link);

        // Assert
        position.Should().BeNull();
    }

    private class CustomNodeModel : NodeModel
    {
        private readonly IShape _shape;

        public CustomNodeModel(IShape shape, Point? position = null) : base(position)
        {
            _shape = shape;
        }

        public CustomNodeModel(IShape shape, string id, Point? position = null) : base(id, position)
        {
            _shape = shape;
        }

        public override IShape GetShape() => _shape;
    }
}