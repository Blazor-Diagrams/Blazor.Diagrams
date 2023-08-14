using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Links;

public partial class MarkersDemo
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Link Markers";
        LayoutData.Info = "Markers are SVG Paths that you can put at the beginning or at the end of your links.";
        LayoutData.DataChanged();

        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        var node1 = NewNode(50, 50);
        var node2 = NewNode(400, 50);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        var link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.SourceMarker = LinkMarker.Arrow;
        link.TargetMarker = LinkMarker.Arrow;
        link.Labels.Add(new LinkLabelModel(link, "Arrow"));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 160);
        node2 = NewNode(400, 160);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.SourceMarker = LinkMarker.Circle;
        link.TargetMarker = LinkMarker.Circle;
        link.Labels.Add(new LinkLabelModel(link, "Circle"));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 270);
        node2 = NewNode(400, 270);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.SourceMarker = LinkMarker.Square;
        link.TargetMarker = LinkMarker.Square;
        link.Labels.Add(new LinkLabelModel(link, "Square"));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 380);
        node2 = NewNode(400, 380);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.SourceMarker = LinkMarker.NewRectangle(10, 20);
        link.TargetMarker = LinkMarker.NewArrow(20, 10);
        link.Labels.Add(new LinkLabelModel(link, "Factory"));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 490);
        node2 = NewNode(400, 490);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.SourceMarker = new LinkMarker("M 0 -8 L 3 -8 3 8 0 8 z M 4 -8 7 -8 7 8 4 8 z M 8 -8 16 0 8 8 z", 16);
        link.TargetMarker = new LinkMarker("M 0 -8 L 8 -8 4 0 8 8 0 8 4 0 z", 8);
        link.Labels.Add(new LinkLabelModel(link, "Custom"));
        _blazorDiagram.Links.Add(link);
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
