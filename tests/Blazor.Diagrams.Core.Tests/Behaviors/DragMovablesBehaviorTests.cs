using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Options;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors;

public class DragMovablesBehaviorTests
{
    [Fact]
    public void Behavior_ShouldCallSetPosition()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(50, 50), Times.Once);
    }

    [Theory]
    [InlineData(false, 0, 0, 45, 45)]
    [InlineData(true, 0, 0, 35, 35)]
    [InlineData(false, 3, 3, 45, 45)]
    [InlineData(true, 3, 3, 50, 50)]
    public void Behavior_SnapToGrid_ShouldCallSetPosition(bool gridSnapToCenter, double initialX, double initialY, double deltaX, double deltaY)
    {
        // Arrange
        var diagram = new TestDiagram(new DiagramOptions
        {
            GridSize = 15,
            GridSnapToCenter = gridSnapToCenter
        });
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var node = diagram.Nodes.Add(nodeMock.Object);
        node.Size = new Size(20, 20);
        node.Position = new Point(initialX, initialY);
        diagram.SelectModel(node, false);

        // Act
        //Move 40px in X and Y
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(20, 20, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(60, 60, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(deltaX, deltaY), Times.Once);
    }

    [Fact]
    public void Behavior_ShouldTriggerMoved()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel(Point.Zero));
        var movedTrigger = false;
        node.Moved += m => movedTrigger = true;
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        movedTrigger.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldNotTriggerMoved_WhenMovableDidntMove()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel(Point.Zero));
        var movedTrigger = false;
        node.Moved += m => movedTrigger = true;
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        movedTrigger.Should().BeFalse();
    }

    [Fact]
    public void Behavior_ShouldNotCallSetPosition_WhenGroupHasNoAutoSize()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var group = new GroupModel(new[] { nodeMock.Object }, autoSize: false);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(50, 50), Times.Never);
    }

    [Fact]
    public void Behavior_ShouldCallSetPosition_WhenGroupHasAutoSize()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var group = new GroupModel(new[] { nodeMock.Object }, autoSize: true);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(50, 50), Times.Once);
    }
}