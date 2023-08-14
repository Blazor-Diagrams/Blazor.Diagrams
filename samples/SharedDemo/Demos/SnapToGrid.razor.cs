using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Options;
using Microsoft.AspNetCore.Components;

namespace SharedDemo;

public class SnapToGridComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new(new BlazorDiagramOptions
    {
        GridSize = 75
    });

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        BlazorDiagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Bottom);
        node.AddPort(PortAlignment.Top);
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        return node;
    }
}
