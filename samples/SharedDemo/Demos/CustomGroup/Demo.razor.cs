using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.CustomGroup;

partial class Demo
{
    private readonly BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Custom group";
        LayoutData.Info = "Creating your own custom groups is very easy!";
        LayoutData.DataChanged();

        _blazorDiagram.Options.LinksLayerOrder = 2;
        _blazorDiagram.Options.NodesLayerOrder = 1;
        _blazorDiagram.RegisterComponent<CustomGroupModel, CustomGroupWidget>();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(300, 300);
        var node3 = NewNode(500, 100);

        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3 });
        _blazorDiagram.Groups.Add(new CustomGroupModel(new[] { node2, node3 }, "Group 1"));

        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        _blazorDiagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
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
