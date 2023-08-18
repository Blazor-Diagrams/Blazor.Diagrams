using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos;

public class GroupingComponent : ComponentBase
{
    protected readonly BlazorDiagram BlazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        BlazorDiagram.Options.Groups.Enabled = true;
        BlazorDiagram.Options.LinksLayerOrder = 2;
        BlazorDiagram.Options.NodesLayerOrder = 1;
        var node1 = NewNode(50, 50);
        var node2 = NewNode(250, 250);
        var node3 = NewNode(500, 100);
        var node4 = NewNode(700, 350);
        BlazorDiagram.Nodes.Add(new[] { node1, node2, node3 });

        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        BlazorDiagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
        BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));

        var group1 = BlazorDiagram.Groups.Group(node1, node2);
        var group2 = BlazorDiagram.Groups.Group(group1, node3);

        BlazorDiagram.Nodes.Add(node4);

        BlazorDiagram.Links.Add(new LinkModel(group2, node4));
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
