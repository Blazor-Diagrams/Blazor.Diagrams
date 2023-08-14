using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors;

public class DragNewLinkBehaviorTests
{
    [Fact]
    public void Behavior_ShouldCreateLinkWithSinglePortAnchorSource_WhenMouseDownOnPort()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(100, 50));
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var link = diagram.Links.Single();
        var source = link.Source as SinglePortAnchor;
        source.Should().NotBeNull();
        source!.Port.Should().BeSameAs(port);
        var ongoingPosition = (link.Target as PositionAnchor)!.GetPlainPosition()!;
        ongoingPosition.X.Should().Be(100);
        ongoingPosition.Y.Should().Be(100);
    }

    [Fact]
    public void Behavior_ShouldCreateLinkUsingFactory_WhenMouseDownOnPort()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var factoryCalled = false;
        diagram.Options.Links.Factory = (d, s, ta) =>
        {
            factoryCalled = true;
            return new LinkModel(new SinglePortAnchor((s as PortModel)!), ta);
        };
        var node = new NodeModel(position: new Point(100, 50));
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        factoryCalled.Should().BeTrue();
        var link = diagram.Links.Single();
        var source = link.Source as SinglePortAnchor;
        source.Should().NotBeNull();
        source!.Port.Should().BeSameAs(port);
        var ongoingPosition = (link.Target as PositionAnchor)!.GetPlainPosition()!;
        ongoingPosition.X.Should().Be(100);
        ongoingPosition.Y.Should().Be(100);
    }

    [Fact]
    public void Behavior_ShouldUpdateOngoingPosition_WhenMouseMoveIsTriggered()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(100, 50));
        var linkRefreshed = false;
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        var link = diagram.Links.Single();
        link.Changed += _ => linkRefreshed = true;
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var source = link.Source as SinglePortAnchor;
        var ongoingPosition = (link.Target as PositionAnchor)!.GetPlainPosition()!;
        ongoingPosition.X.Should().BeGreaterThan(145);
        ongoingPosition.Y.Should().BeGreaterThan(145);
        linkRefreshed.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldUpdateOngoingPosition_WhenMouseMoveIsTriggeredAndZoomIsChanged()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        diagram.SetZoom(1.5);
        var node = new NodeModel(position: new Point(100, 50));
        var linkRefreshed = false;
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        var link = diagram.Links.Single();
        link.Changed += _ => linkRefreshed = true;
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(160, 160, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var source = link.Source as SinglePortAnchor;
        var ongoingPosition = (link.Target as PositionAnchor)!.GetPlainPosition()!;
        ongoingPosition.X.Should().BeApproximately(107.7, 0.1);
        ongoingPosition.Y.Should().BeApproximately(101.7, 0.1);
        linkRefreshed.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldSnapToClosestPortAndRefreshPort_WhenSnappingIsEnabledAndPortIsInRadius()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        diagram.Options.Links.EnableSnapping = true;
        diagram.Options.Links.SnappingRadius = 60;
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });
        var port2Refreshed = false;
        port2.Changed += _ => port2Refreshed = true;

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(140, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var link = diagram.Links.Single();
        var target = link.Target as SinglePortAnchor;
        target.Should().NotBeNull();
        target!.Port.Should().BeSameAs(port2);
        port2Refreshed.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldNotSnapToPort_WhenSnappingIsEnabledAndPortIsNotInRadius()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        diagram.Options.Links.EnableSnapping = true;
        diagram.Options.Links.SnappingRadius = 50;
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(105, 105, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var link = diagram.Links.Single();
        link.Target.Should().BeOfType<PositionAnchor>();
    }

    [Fact]
    public void Behavior_ShouldUnSnapAndRefreshPort_WhenSnappingIsEnabledAndPortIsNotInRadiusAnymore()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        diagram.Options.Links.EnableSnapping = true;
        diagram.Options.Links.SnappingRadius = 56;
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });
        var port2Refreshes = 0;
        port2.Changed += _ => port2Refreshes++;

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(140, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty,
                true)); // Move towards the other port
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty,
                true)); // Move back to unsnap

        // Assert
        var link = diagram.Links.Single();
        var target = link.Target as SinglePortAnchor;
        target.Should().BeNull();
        port2Refreshes.Should().Be(2);
    }

    [Fact]
    public void Behavior_ShouldRemoveLink_WhenMouseUpOnCanvasAndRequireTargetIsTrue()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(100, 50));
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null,
            new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        diagram.Links.Should().BeEmpty();
    }

    [Fact]
    public void Behavior_ShouldRemoveLink_WhenMouseUpOnSamePort()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(100, 50));
        var port = node.AddPort(new PortModel(node)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port,
            new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(port,
            new PointerEventArgs(0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        diagram.Links.Should().BeEmpty();
    }

    [Fact]
    public void Behavior_ShouldSetTarget_WhenMouseUp()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });
        var port2Refreshes = 0;
        port2.Changed += _ => port2Refreshes++;

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(port2,
            new PointerEventArgs(105, 105, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        var link = diagram.Links.Single();
        var target = link.Target as SinglePortAnchor;
        target.Should().NotBeNull();
        target!.Port.Should().BeSameAs(port2);
        port2Refreshes.Should().Be(1);
    }

    [Fact]
    public void Behavior_ShouldNotCreateOngoingLink_WhenFactoryReturnsNull()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.Options.Links.Factory = (d, s, ta) => null;
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));

        var node1 = new NodeModel(position: new Point(100, 50));
        diagram.Nodes.Add(node1);
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        diagram.Links.Should().HaveCount(0);
    }

    [Fact]
    public void Behavior_ShouldTriggerLinkTargetAttached_WhenMouseUpOnOtherPort()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });
        var targetAttachedTriggers = 0;

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        diagram.Links.Single().TargetAttached += _ => targetAttachedTriggers++;

        diagram.TriggerPointerUp(port2,
            new PointerEventArgs(105, 105, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        targetAttachedTriggers.Should().Be(1);
    }

    [Fact]
    public void Behavior_ShouldTriggerLinkTargetAttached_WhenLinkSnappedToPortAndMouseUp()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        diagram.Options.Links.EnableSnapping = true;
        diagram.Options.Links.SnappingRadius = 60;
        var node1 = new NodeModel(position: new Point(100, 50));
        var node2 = new NodeModel(position: new Point(160, 50));
        diagram.Nodes.Add(new[] { node1, node2 });
        var port1 = node1.AddPort(new PortModel(node1)
        {
            Initialized = true,
            Position = new Point(110, 60),
            Size = new Size(10, 20)
        });
        var port2 = node2.AddPort(new PortModel(node2)
        {
            Initialized = true,
            Position = new Point(170, 60),
            Size = new Size(10, 20)
        });
        var targetAttachedTriggers = 0;

        // Act
        diagram.TriggerPointerDown(port1,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        diagram.TriggerPointerMove(null,
            new PointerEventArgs(140, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        diagram.Links.Single().TargetAttached += _ => targetAttachedTriggers++;

        diagram.TriggerPointerUp(null,
            new PointerEventArgs(140, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        targetAttachedTriggers.Should().Be(1);
    }
}