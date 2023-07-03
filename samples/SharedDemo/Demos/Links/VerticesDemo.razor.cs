using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;

namespace SharedDemo.Demos.Links;

public partial class VerticesDemo
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Link Vertices";
        LayoutData.Info = "Click on a link to create a vertex. Double click on a vertex to delete it. " +
            "You can drag the vertices around.";
        LayoutData.DataChanged();

        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        var node1 = NewNode(50, 80);
        var node2 = NewNode(200, 350);
        var node3 = NewNode(400, 100);

        _blazorDiagram.Nodes.Add(new[] { node1, node2, node3 });

        var link1 = new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left))
        {
            PathGenerator = new StraightPathGenerator(),
            Segmentable = true
        };
        link1.Labels.Add(new LinkLabelModel(link1, "Content"));
        link1.Vertices.Add(new LinkVertexModel(link1, new Point(221, 117)));
        link1.Vertices.Add(new LinkVertexModel(link1, new Point(111, 291)));

        var link2 = new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Left))
        {
            PathGenerator = new SmoothPathGenerator(), // default
            Segmentable = true
        };
        link2.Labels.Add(new LinkLabelModel(link2, "Content"));
        link2.Vertices.Add(new LinkVertexModel(link2, new Point(400, 324)));
        link2.Vertices.Add(new LinkVertexModel(link2, new Point(326, 180)));

        _blazorDiagram.Links.Add(new[] { link1, link2 });
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
