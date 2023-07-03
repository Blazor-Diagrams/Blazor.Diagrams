using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Blazor.Diagrams.Core.Tests;

public class DiagramOrderingTests
{
    [Fact]
    public void GetMinOrder_ShouldReturnZeroWhenNoModelsHaveBeenAdded()
    {
        // Arrange
        var diagram = new TestDiagram();

        // Act
        var minOrder = diagram.GetMinOrder();

        // Assert
        minOrder.Should().Be(0);
    }

    [Fact]
    public void GetMinOrder_ShouldReturnCorrectValue()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.Nodes.Add(new NodeModel());
        diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        diagram.Links.Add(new LinkModel(diagram.Nodes[0], diagram.Groups[0]));

        // Act
        var minOrder = diagram.GetMinOrder();

        // Assert
        minOrder.Should().Be(1);
    }

    [Fact]
    public void GetMaxOrder_ShouldReturnZeroWhenNoModelsHaveBeenAdded()
    {
        // Arrange
        var diagram = new TestDiagram();

        // Act
        var maxOrder = diagram.GetMaxOrder();

        // Assert
        maxOrder.Should().Be(0);
    }

    [Fact]
    public void GetMaxOrder_ShouldReturnCorrectValue()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.Nodes.Add(new NodeModel());
        diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        diagram.Links.Add(new LinkModel(diagram.Nodes[0], diagram.Groups[0]));

        // Act
        var maxOrder = diagram.GetMaxOrder();

        // Assert
        maxOrder.Should().Be(3);
    }

    [Fact]
    public void Diagram_ShouldReSortWhenModelOrderChanges()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel()); // 1
        var node2 = diagram.Nodes.Add(new NodeModel()); // 2

        // Act
        node1.Order = 10;

        // Assert
        diagram.OrderedSelectables[0].Should().Be(node2);
        diagram.OrderedSelectables[1].Should().Be(node1);
    }

    [Fact]
    public void Diagram_ShouldRefreshOnceWhenModelOrderChanges()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel()); // 1
        var node2 = diagram.Nodes.Add(new NodeModel()); // 2
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        node1.Order = 10;

        // Assert
        refreshes.Should().Be(1);
    }

    [Fact]
    public void SendToBack_ShouldInsertAtZeroAndFixOrders()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var node2 = diagram.Nodes.Add(new NodeModel());
        var node3 = diagram.Nodes.Add(new NodeModel());

        // Act
        diagram.SendToBack(node3);

        // Assert
        diagram.OrderedSelectables[0].Should().Be(node3);
        diagram.OrderedSelectables[0].Order.Should().Be(1);

        diagram.OrderedSelectables[1].Should().Be(node1);
        diagram.OrderedSelectables[1].Order.Should().Be(2);

        diagram.OrderedSelectables[2].Should().Be(node2);
        diagram.OrderedSelectables[2].Order.Should().Be(3);
    }

    [Fact]
    public void SendToFront_ShouldAddAndFixOrder()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var node2 = diagram.Nodes.Add(new NodeModel());
        var node3 = diagram.Nodes.Add(new NodeModel());

        // Act
        diagram.SendToFront(node1);

        // Assert
        diagram.OrderedSelectables[0].Should().Be(node2);
        diagram.OrderedSelectables[0].Order.Should().Be(2);

        diagram.OrderedSelectables[1].Should().Be(node3);
        diagram.OrderedSelectables[1].Order.Should().Be(3);

        diagram.OrderedSelectables[2].Should().Be(node1);
        diagram.OrderedSelectables[2].Order.Should().Be(4);
    }

    [Fact]
    public void Diagram_ShouldRefreshOnceWhenMultipleModelsWereRemoved()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var node2 = diagram.Nodes.Add(new NodeModel());
        var link = diagram.Links.Add(new LinkModel(node1, node2));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Nodes.Remove(node1);

        // Assert
        refreshes.Should().Be(1);
    }

    [Fact]
    public void Diagram_ShouldNotUpdateOrders_WhenSuspendSortingIsTrue()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SuspendSorting = true;
        var node1 = diagram.Nodes.Add(new NodeModel()); // 1
        var node2 = diagram.Nodes.Add(new NodeModel()); // 2

        // Act
        node1.Order = 10;

        // Assert
        diagram.OrderedSelectables[0].Should().Be(node1);
        diagram.OrderedSelectables[1].Should().Be(node2);
    }

    [Fact]
    public void RefreshOrders_ShouldSortModels()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel() { Order = 10 });
        var node2 = diagram.Nodes.Add(new NodeModel() { Order = 5 });

        // Act
        diagram.RefreshOrders();

        // Assert
        diagram.OrderedSelectables[0].Should().Be(node2);
        diagram.OrderedSelectables[1].Should().Be(node1);
    }
}
