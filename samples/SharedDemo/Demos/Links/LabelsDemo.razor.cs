using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.Links;

public partial class LabelsDemo
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Link Labels";
        LayoutData.Info = "Labels help you show more information through out a link. You can specify a distance or an offset. <br>" +
            "The content of the labels is still limited because of Blazor's poor SVG support.";
        LayoutData.DataChanged();

        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        var node1 = NewNode(50, 50);
        var node2 = NewNode(400, 50);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        var link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.Labels.Add(new LinkLabelModel(link, "Content"));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 160);
        node2 = NewNode(400, 160);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.Labels.Add(new LinkLabelModel(link, "0.25", 0.3));
        link.Labels.Add(new LinkLabelModel(link, "0.75", 0.7));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 270);
        node2 = NewNode(400, 270);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.Labels.Add(new LinkLabelModel(link, "50", 50));
        link.Labels.Add(new LinkLabelModel(link, "-50", -50));
        _blazorDiagram.Links.Add(link);

        node1 = NewNode(50, 380);
        node2 = NewNode(400, 380);

        _blazorDiagram.Nodes.Add(new[] { node1, node2 });

        link = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left));
        link.Labels.Add(new LinkLabelModel(link, "(0,-20)", 50, new Point(0, -20)));
        link.Labels.Add(new LinkLabelModel(link, "(0,20)", -50, new Point(0, 20)));
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
