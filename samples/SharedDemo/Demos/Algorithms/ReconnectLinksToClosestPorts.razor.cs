using Blazor.Diagrams;
using Blazor.Diagrams.Algorithms;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos.Algorithms;

public class ReconnectLinksToClosestPortsComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        var node3 = NewNode(300, 50);
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Top), node2.GetPort(PortAlignment.Right)));
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Bottom), node3.GetPort(PortAlignment.Top)));
        BlazorDiagram.Nodes.Add(new[] { node1, node2, node3 });
    }


    protected void ReconnectLinks() => BlazorDiagram.ReconnectLinksToClosestPorts();


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
