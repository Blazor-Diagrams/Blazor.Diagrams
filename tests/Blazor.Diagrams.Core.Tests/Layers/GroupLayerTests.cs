using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Layers;

public class GroupLayerTests
{
    [Fact]
    public void Group_ShouldCallFactoryThenAddMethod()
    {
        // Arrange
        var diagram = new TestDiagram();
        var factoryCalled = false;

        diagram.Options.Groups.Factory = (_, children) =>
        {
            factoryCalled = true;
            return new GroupModel(children);
        };

        // Act
        diagram.Groups.Group(Array.Empty<NodeModel>());

        // Assert
        factoryCalled.Should().BeTrue();
    }

    [Fact]
    public void Remove_ShouldRemoveAllPortLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var groupPort = group.AddPort(PortAlignment.Top);
        var node = diagram.Nodes.Add(new NodeModel());
        var nodePort = node.AddPort(PortAlignment.Top);
        diagram.Links.Add(new LinkModel(groupPort, nodePort));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        diagram.Links.Should().BeEmpty();
    }

    [Fact]
    public void Remove_ShouldRemoveAllLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var node = diagram.Nodes.Add(new NodeModel());
        diagram.Links.Add(new LinkModel(group, node));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        diagram.Links.Should().BeEmpty();
    }

    [Fact]
    public void Remove_ShouldRemoveItselfFromParentGroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group1 = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var group2 = diagram.Groups.Add(new GroupModel(new[] { group1 }));

        // Act
        diagram.Groups.Remove(group1);

        // Assert
        group2.Children.Should().BeEmpty();
        group1.Group.Should().BeNull();
    }

    [Fact]
    public void Remove_ShouldUngroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        group.Children.Should().BeEmpty();
        node.Group.Should().BeNull();
    }

    [Fact]
    public void Delete_ShouldDeleteChildGroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group1 = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var group2 = diagram.Groups.Add(new GroupModel(new[] { group1 }));

        // Act
        diagram.Groups.Delete(group2);

        // Assert
        diagram.Groups.Should().BeEmpty();
    }

    [Fact]
    public void Delete_ShouldRemoveChild()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));

        // Act
        diagram.Groups.Delete(group);

        // Assert
        diagram.Groups.Should().BeEmpty();
        diagram.Nodes.Should().BeEmpty();
    }

    [Fact]
    public void Add_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));

        // Assert
        refreshes.Should().Be(1);
    }

    [Fact]
    public void Remove_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Groups.Remove(group);

        // Assert
        refreshes.Should().Be(1);
    }

    [Fact]
    public void Delete_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Groups.Delete(group);

        // Assert
        refreshes.Should().Be(1);
    }
}
