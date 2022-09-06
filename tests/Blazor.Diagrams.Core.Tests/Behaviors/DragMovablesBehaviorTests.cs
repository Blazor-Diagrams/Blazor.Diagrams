using System.Linq;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
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
}