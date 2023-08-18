using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo;

public class LockedComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        BlazorDiagram.Nodes.Add(new[] { node1, node2, NewNode(300, 50) });

        var link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left))
        {
            Locked = true
        };
        BlazorDiagram.Links.Add(link);
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Bottom);
        node.AddPort(PortAlignment.Top).Locked = true;
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        node.Locked = true;
        return node;
    }
}
