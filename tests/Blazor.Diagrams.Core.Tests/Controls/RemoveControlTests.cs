using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Controls
{
    public class RemoveControlTests
    {
        public PointerEventArgs PointerEventArgs
            => new(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true);

        [Fact]
        public async Task OnPointerDown_NoConstraints_RemovesNode()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram();
            var nodeMock = new Mock<NodeModel>(Point.Zero);
            var node = diagram.Nodes.Add(nodeMock.Object);

            // Act
            await removeControl.OnPointerDown(diagram, node, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Nodes);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteNodeTrue_RemovesNode()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteNode = (node) => ValueTask.FromResult(true)
                    }
                });
            var nodeMock = new Mock<NodeModel>(Point.Zero);
            var node = diagram.Nodes.Add(nodeMock.Object);

            // Act
            await removeControl.OnPointerDown(diagram, node, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Nodes);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteNodeFalse_KeepsNode()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteNode = (node) => ValueTask.FromResult(false)
                    }
                });
            var nodeMock = new Mock<NodeModel>(Point.Zero);
            var node = diagram.Nodes.Add(nodeMock.Object);

            // Act
            await removeControl.OnPointerDown(diagram, node, PointerEventArgs);

            // Assert
            Assert.Contains(node, diagram.Nodes);
        }

        [Fact]
        public async Task OnPointerDown_NoConstraints_RemovesLink()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram();

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var link = new LinkModel(
                    node1.GetPort(PortAlignment.Right)!,
                    node2.GetPort(PortAlignment.Left)!
                    );

            diagram.Links.Add(link);

            // Act
            await removeControl.OnPointerDown(diagram, link, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Links);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteLinkTrue_RemovesLink()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteLink = (node) => ValueTask.FromResult(true)
                    }
                });

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var link = new LinkModel(
                    node1.GetPort(PortAlignment.Right)!,
                    node2.GetPort(PortAlignment.Left)!
                    );

            diagram.Links.Add(link);

            // Act
            await removeControl.OnPointerDown(diagram, link, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Links);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteLinkFalse_KeepsLink()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteLink = (node) => ValueTask.FromResult(false)
                    }
                });

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var link = new LinkModel(
                    node1.GetPort(PortAlignment.Right)!,
                    node2.GetPort(PortAlignment.Left)!
                    );

            diagram.Links.Add(link);

            // Act
            await removeControl.OnPointerDown(diagram, link, PointerEventArgs);

            // Assert
            Assert.Contains(link, diagram.Links);
        }

        [Fact]
        public async Task OnPointerDown_NoConstraints_RemovesGroup()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram();

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var group = new GroupModel(new[] { node1, node2 });


            diagram.Groups.Add(group);

            // Act
            await removeControl.OnPointerDown(diagram, group, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Groups);
            Assert.Empty(diagram.Nodes);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteGroupTrue_RemovesGroup()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteGroup = (node) => ValueTask.FromResult(true)
                    }
                });

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var group = new GroupModel(new[] { node1, node2 });


            diagram.Groups.Add(group);

            // Act
            await removeControl.OnPointerDown(diagram, group, PointerEventArgs);

            // Assert
            Assert.Empty(diagram.Groups);
            Assert.Empty(diagram.Nodes);
        }

        [Fact]
        public async Task OnPointerDown_ShouldDeleteGroupFalse_KeepsGroup()
        {
            // Arrange
            RemoveControl removeControl = new(0, 0);
            Diagram diagram = new TestDiagram(
                new Options.DiagramOptions
                {
                    Constraints =
                    {
                        ShouldDeleteGroup = (node) => ValueTask.FromResult(false)
                    }
                });

            var node1 = new NodeModel(new Point(50, 50));
            var node2 = new NodeModel(new Point(300, 300));
            diagram.Nodes.Add(new[] { node1, node2 });
            node1.AddPort(PortAlignment.Right);
            node2.AddPort(PortAlignment.Left);

            var group = new GroupModel(new[] { node1, node2 });


            diagram.Groups.Add(group);

            // Act
            await removeControl.OnPointerDown(diagram, group, PointerEventArgs);

            // Assert
            Assert.Contains(group, diagram.Groups);
            Assert.Contains(node1, diagram.Nodes);
            Assert.Contains(node2, diagram.Nodes);
        }

    }
}
