using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class DragNewLinkBehaviorTests
    {
        [Fact]
        public void Behavior_ShouldCreateLinkWithSinglePortAnchorSource_WhenMouseDownOnPort()
        {
            // Arrange
            var diagram = new DiagramBase();
            var node = new NodeModel(position: new Point(100, 50));
            var port = node.AddPort(new PortModel(node)
            {
                Initialized = true,
                Position = new Point(110, 60),
                Size = new Size(10, 20)
            });

            // Act
            diagram.OnMouseDown(port, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            var link = diagram.Links.Single();
            var source = link.Source as SinglePortAnchor;
            source.Should().NotBeNull();
            link.Target.Should().BeNull();
            source!.Port.Should().BeSameAs(port);
            link.OnGoingPosition.Should().NotBeNull();
            var sourcePosition = source.GetPosition(link)!;
            link.OnGoingPosition!.X.Should().Be(sourcePosition.X);
            link.OnGoingPosition.Y.Should().Be(sourcePosition.Y);
        }

        [Fact]
        public void Behavior_ShouldCreateLinkUsingFactory_WhenMouseDownOnPort()
        {
            // Arrange
            var diagram = new DiagramBase();
            var factoryCalled = false;
            diagram.Options.Links.Factory = (d, sp) =>
            {
                factoryCalled = true;
                return new LinkModel(sp);
            };
            var node = new NodeModel(position: new Point(100, 50));
            var port = node.AddPort(new PortModel(node)
            {
                Initialized = true,
                Position = new Point(110, 60),
                Size = new Size(10, 20)
            });

            // Act
            diagram.OnMouseDown(port, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            factoryCalled.Should().BeTrue();
            var link = diagram.Links.Single();
            var source = link.Source as SinglePortAnchor;
            source.Should().NotBeNull();
            link.Target.Should().BeNull();
            source!.Port.Should().BeSameAs(port);
            link.OnGoingPosition.Should().NotBeNull();
            var sourcePosition = source.GetPosition(link)!;
            link.OnGoingPosition!.X.Should().Be(sourcePosition.X);
            link.OnGoingPosition.Y.Should().Be(sourcePosition.Y);
        }

        [Fact]
        public void Behavior_ShouldUpdateOngoingPosition_WhenMouseMoveIsTriggered()
        {
            // Arrange
            var diagram = new DiagramBase();
            var node = new NodeModel(position: new Point(100, 50));
            var linkRefreshed = false;
            var port = node.AddPort(new PortModel(node)
            {
                Initialized = true,
                Position = new Point(110, 60),
                Size = new Size(10, 20)
            });

            // Act
            diagram.OnMouseDown(port, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            var link = diagram.Links.Single();
            link.Changed += () => linkRefreshed = true;
            diagram.OnMouseMove(null, new MouseEventArgs(150, 150, 0, 0, false, false, false));

            // Assert
            var source = link.Source as SinglePortAnchor;
            var sourcePosition = source!.GetPosition(link)!;
            link.OnGoingPosition!.X.Should().Be(sourcePosition.X + 50);
            link.OnGoingPosition.Y.Should().Be(sourcePosition.Y + 50);
            linkRefreshed.Should().BeTrue();
        }

        [Fact]
        public void Behavior_ShouldUpdateOngoingPosition_WhenMouseMoveIsTriggeredAndZoomIsChanged()
        {
            // Arrange
            var diagram = new DiagramBase();
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
            diagram.OnMouseDown(port, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            var link = diagram.Links.Single();
            link.Changed += () => linkRefreshed = true;
            diagram.OnMouseMove(null, new MouseEventArgs(160, 160, 0, 0, false, false, false));

            // Assert
            var source = link.Source as SinglePortAnchor;
            var sourcePosition = source!.GetPosition(link)!;
            link.OnGoingPosition!.X.Should().Be(sourcePosition.X + 40);
            link.OnGoingPosition.Y.Should().Be(sourcePosition.Y + 40);
            linkRefreshed.Should().BeTrue();
        }

        [Fact]
        public void Behavior_ShouldSnapToClosestPortAndRefreshPort_WhenSnappingIsEnabledAndPortIsInRadius()
        {
            // Arrange
            var diagram = new DiagramBase();
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
            port2.Changed += () => port2Refreshed = true;

            // Act
            diagram.OnMouseDown(port1, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            diagram.OnMouseMove(null, new MouseEventArgs(105, 105, 0, 0, false, false, false));

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
            var diagram = new DiagramBase();
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
            diagram.OnMouseDown(port1, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            diagram.OnMouseMove(null, new MouseEventArgs(105, 105, 0, 0, false, false, false));

            // Assert
            var link = diagram.Links.Single();
            link.Target.Should().BeNull();
        }

        [Fact]
        public void Behavior_ShouldUnSnapAndRefreshPort_WhenSnappingIsEnabledAndPortIsNotInRadiusAnymore()
        {
            // Arrange
            var diagram = new DiagramBase();
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
            port2.Changed += () => port2Refreshes++;

            // Act
            diagram.OnMouseDown(port1, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            diagram.OnMouseMove(null, new MouseEventArgs(105, 105, 0, 0, false, false, false)); // Move towards the other port
            diagram.OnMouseMove(null, new MouseEventArgs(100, 100, 0, 0, false, false, false)); // Move back to unsnap

            // Assert
            var link = diagram.Links.Single();
            var target = link.Target as SinglePortAnchor;
            target.Should().BeNull();
            port2Refreshes.Should().Be(2);
        }

        [Fact]
        public void Behavior_ShouldRemoveLink_WhenMouseUpOnCanvas()
        {
            // Arrange
            var diagram = new DiagramBase();
            var node = new NodeModel(position: new Point(100, 50));
            var port = node.AddPort(new PortModel(node)
            {
                Initialized = true,
                Position = new Point(110, 60),
                Size = new Size(10, 20)
            });

            // Act
            diagram.OnMouseDown(port, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseUp(null, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            diagram.Links.Should().BeEmpty();
        }

        [Fact]
        public void Behavior_ShouldRemoveLink_WhenMouseUpOnSamePort()
        {
            // Arrange
            var diagram = new DiagramBase();
            var node = new NodeModel(position: new Point(100, 50));
            var port = node.AddPort(new PortModel(node)
            {
                Initialized = true,
                Position = new Point(110, 60),
                Size = new Size(10, 20)
            });

            // Act
            diagram.OnMouseDown(port, new MouseEventArgs(0, 0, 0, 0, false, false, false));
            diagram.OnMouseUp(port, new MouseEventArgs(0, 0, 0, 0, false, false, false));

            // Assert
            diagram.Links.Should().BeEmpty();
        }

        [Fact]
        public void Behavior_ShouldSetTarget_WhenMouseUp()
        {
            // Arrange
            var diagram = new DiagramBase();
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
            port2.Changed += () => port2Refreshes++;

            // Act
            diagram.OnMouseDown(port1, new MouseEventArgs(100, 100, 0, 0, false, false, false));
            diagram.OnMouseUp(port2, new MouseEventArgs(105, 105, 0, 0, false, false, false));

            // Assert
            var link = diagram.Links.Single();
            link.OnGoingPosition.Should().BeNull();
            var target = link.Target as SinglePortAnchor;
            target.Should().NotBeNull();
            target!.Port.Should().BeSameAs(port2);
            port2Refreshes.Should().Be(1);
        }
    }
}
