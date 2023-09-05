using Blazor.Diagrams;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Models;

namespace SharedDemo.Demos.Nodes;

public partial class SvgDemo
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "SVG Nodes";
        LayoutData.Info = "You can also have SVG nodes! All you need to do is to set the Layer to RenderLayer.SVG.";
        LayoutData.DataChanged();

        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        _blazorDiagram.RegisterComponent<NodeModel, NodeWidget>();
        _blazorDiagram.RegisterComponent<SvgNodeModel, SvgNodeWidget>();
        _blazorDiagram.RegisterComponent<SvgGroupModel, DefaultGroupWidget>();

        var node1 = NewNode(50, 50);
        var node2 = NewNode(250, 250);
        var node3 = NewNode(500, 100);
        var node4 = NewNode(700, 350);
        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3, node4 });

        var group1 = _blazorDiagram.Groups.Add(new SvgGroupModel(new[] { node1, node2 }));
        var group2 = _blazorDiagram.Groups.Add(new SvgGroupModel(new[] { group1, node3 }));

        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        _blazorDiagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
        _blazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left)));
        var link = _blazorDiagram.Links.Add(new LinkModel(group2, node4));

        var controls1 = _blazorDiagram.Controls.AddFor(node4);
        controls1.Add(new RemoveControl(1, 0));
        controls1.Add(new BoundaryControl());

        var controls2 = _blazorDiagram.Controls.AddFor(link);
        controls2.Add(new RemoveControl(1, 0));
        controls2.Add(new BoundaryControl());
    }

    private NodeModel NewNode(double x, double y, bool svg = true)
    {
        var node = svg ? new SvgNodeModel(new Point(x, y)) : new NodeModel(new Point(x, y));
        node.AddPort(PortAlignment.Left);
        node.AddPort(PortAlignment.Right);
        return node;
    }
}
