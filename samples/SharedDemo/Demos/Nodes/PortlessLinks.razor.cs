using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Controls.Default;

namespace SharedDemo.Demos.Nodes;

public partial class PortlessLinks
{
    private BlazorDiagram _blazorDiagram = new BlazorDiagram();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        LayoutData.Title = "Portless Links";
        LayoutData.Info = "Starting from 2.0, you can create links between nodes directly! " +
                          "All you need to specify is the shape of your nodes in order to calculate the connection points.";
        LayoutData.DataChanged();

        InitializeDiagram();
    }

    private void InitializeDiagram()
    {
        _blazorDiagram.RegisterComponent<RoundedNode, RoundedNodeWidget>();

        var node1 = new NodeModel(new Point(80, 80));
        var node2 = new RoundedNode(new Point(280, 150));
        var node3 = new NodeModel(new Point(400, 300));
        node3.AddPort(PortAlignment.Left);
        _blazorDiagram.Nodes.Add(node1);
        _blazorDiagram.Nodes.Add(node2);
        _blazorDiagram.Nodes.Add(node3);
        _blazorDiagram.Links.Add(new LinkModel(node1, node2)
        {
            SourceMarker = LinkMarker.Arrow,
            TargetMarker = LinkMarker.Arrow,
            Segmentable = true
        });
        _blazorDiagram.Links.Add(new LinkModel(new ShapeIntersectionAnchor(node2),
            new SinglePortAnchor(node3.GetPort(PortAlignment.Left)))
        {
            SourceMarker = LinkMarker.Arrow,
            TargetMarker = LinkMarker.Arrow,
            Segmentable = true
        });

        _blazorDiagram.Controls.AddFor(node1)
            .Add(new RemoveControl(1, 0))
            .Add(new DragNewLinkControl(1, 0.5, 20))
            .Add(new BoundaryControl());

        _blazorDiagram.Controls.AddFor(node2)
            .Add(new RemoveControl(1, 0))
            .Add(new DragNewLinkControl(1, 0.5, 20))
            .Add(new BoundaryControl());

        _blazorDiagram.Controls.AddFor(node3)
            .Add(new RemoveControl(1, 0))
            .Add(new DragNewLinkControl(1, 0.5, 20))
            .Add(new BoundaryControl());
    }
}

class RoundedNode : NodeModel
{
    public RoundedNode(Point position = null) : base(position)
    {
    }

    public override IShape GetShape()
    {
        return Shapes.Circle(this);
    }
}