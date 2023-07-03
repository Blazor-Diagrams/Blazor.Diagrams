using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Tests.TestComponents;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class LinkVertexWidgetTests
{
    [Fact]
    public void ShouldRenderCircle()
    {
        // Arrange
        using var ctx = new TestContext();
        var node1 = new NodeModel();
        var node2 = new NodeModel();
        var link = new LinkModel(node1, node2);
        var vertex = new LinkVertexModel(link, new Point(10.5, 20));
        link.Vertices.Add(vertex);

        // Act
        var cut = ctx.RenderComponent<LinkVertexRenderer>(parameters => parameters
            .Add(n => n.Vertex, vertex)
            .Add(n => n.Color, "red")
            .Add(n => n.SelectedColor, "blue")
            .Add(n => n.BlazorDiagram, new BlazorDiagram()));

        // Assert
        cut.MarkupMatches("<g class=\"diagram-link-vertex\" cursor=\"move\"><circle cx=\"10.5\" cy=\"20\" r=\"5\" fill=\"red\" /></g>");
    }

    [Fact]
    public void ShouldRenderCircleWithSelectedColor_WhenVertexIsSelected()
    {
        // Arrange
        using var ctx = new TestContext();
        var node1 = new NodeModel();
        var node2 = new NodeModel();
        var link = new LinkModel(node1, node2);
        var vertex = new LinkVertexModel(link, new Point(10.5, 20));
        link.Vertices.Add(vertex);
        vertex.Selected = true;

        // Act
        var cut = ctx.RenderComponent<LinkVertexRenderer>(parameters => parameters
            .Add(n => n.Vertex, vertex)
            .Add(n => n.Color, "red")
            .Add(n => n.SelectedColor, "blue")
            .Add(n => n.BlazorDiagram, new BlazorDiagram()));

        // Assert
        cut.MarkupMatches("<g class=\"diagram-link-vertex\" cursor=\"move\"><circle cx=\"10.5\" cy=\"20\" r=\"5\" fill=\"blue\" /></g>");
    }

    [Fact]
    public void ShouldRerender_WhenVertexIsRefreshed()
    {
        // Arrange
        using var ctx = new TestContext();
        var node1 = new NodeModel();
        var node2 = new NodeModel();
        var link = new LinkModel(node1, node2);
        var vertex = new LinkVertexModel(link, new Point(10.5, 20));
        link.Vertices.Add(vertex);

        // Act
        var cut = ctx.RenderComponent<LinkVertexRenderer>(parameters => parameters
            .Add(n => n.Vertex, vertex)
            .Add(n => n.Color, "red")
            .Add(n => n.SelectedColor, "blue")
            .Add(n => n.BlazorDiagram, new BlazorDiagram()));

        // Assert
        cut.RenderCount.Should().Be(1);
        vertex.Refresh();
        cut.RenderCount.Should().Be(2);
    }

    [Fact]
    public async Task ShouldDeleteItselfAndRefreshParent_WhenDoubleClicked()
    {
        // Arrange
        using var ctx = new TestContext();
        var node1 = new NodeModel();
        var node2 = new NodeModel();
        var link = new LinkModel(node1, node2);
        int linkRefreshes = 0;
        var vertex = new LinkVertexModel(link, new Point(10.5, 20));
        link.Vertices.Add(vertex);
        link.Changed += _ => linkRefreshes++;

        // Act
        var cut = ctx.RenderComponent<LinkVertexRenderer>(parameters => parameters
            .Add(n => n.Vertex, vertex)
            .Add(n => n.Color, "red")
            .Add(n => n.SelectedColor, "blue")
            .Add(n => n.BlazorDiagram, new BlazorDiagram()));

        await cut.Find("circle").DoubleClickAsync(new MouseEventArgs());

        // Assert
        link.Vertices.Should().BeEmpty();
        linkRefreshes.Should().Be(1);
    }

    [Fact]
    public void ShouldUseCustomComponent_WhenProvided()
    {
        // Arrange
        using var ctx = new TestContext();
        var diagram = new BlazorDiagram();
        diagram.RegisterComponent<LinkVertexModel, CustomVertexWidget>();
        var node1 = new NodeModel();
        var node2 = new NodeModel();
        var link = new LinkModel(node1, node2);
        var vertex = new LinkVertexModel(link, new Point(10.5, 20));
        link.Vertices.Add(vertex);

        // Act
        var cut = ctx.RenderComponent<LinkVertexRenderer>(parameters => parameters
            .Add(n => n.Vertex, vertex)
            .Add(n => n.Color, "red")
            .Add(n => n.SelectedColor, "blue")
            .Add(n => n.BlazorDiagram, diagram));

        // Assert
        cut.MarkupMatches("<g class=\"diagram-link-vertex\" cursor=\"move\"><circle cx=\"10.5\" cy=\"20\" r=\"10\" fill=\"red\" /></g>");
    }
}
