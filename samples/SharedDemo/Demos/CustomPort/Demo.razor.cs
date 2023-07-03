using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.CustomPort;

partial class Demo
{
    private readonly BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Custom port";
        LayoutData.Info = "Creating your own custom ports is very easy!<br>" +
            "In this example, you can only attach links from/to ports with the same color.";
        LayoutData.DataChanged();

        _blazorDiagram.RegisterComponent<NodeModel, ColoredNodeWidget>(replace: true);

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        _blazorDiagram.Nodes.Add(new[] { node1, node2, NewNode(500, 50) });
        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Top), node2.GetPort(PortAlignment.Top)));
    }

    private NodeModel NewNode(double x, double y)
    {
        var node = new NodeModel(new Point(x, y));
        node.AddPort(new ColoredPort(node, PortAlignment.Top, true));
        node.AddPort(new ColoredPort(node, PortAlignment.Left, false));
        return node;
    }
}
